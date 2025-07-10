using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeraStore.Services.User.Persistence.Configurations;

/// <summary>
/// Configures the EF Core mappings for the User entity.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<Domain.Entities.User>
{
  public void Configure(EntityTypeBuilder<Domain.Entities.User> builder)
  {
    builder.ToTable("Users");

    builder.HasKey(u => u.Id);

    builder.Property(u => u.Id)
      .ValueGeneratedOnAdd();

    builder.Property(u => u.UserName)
      .IsRequired()
      .HasMaxLength(100);

    builder.Property(u => u.Email)
      .IsRequired()
      .HasMaxLength(150);

    builder.Property(u => u.IsDeleted)
      .IsRequired()
      .HasDefaultValue(false);

    builder.Property(u => u.CreatedDate)
      .IsRequired();

    builder.Property(u => u.ModifiedDate)
      .IsRequired(false);

    // Indexes
    builder.HasIndex(u => new { u.UserName, u.IsDeleted }).IsUnique();
    builder.HasIndex(u => new { u.Email, u.IsDeleted }).IsUnique();
    builder.HasIndex(u => u.IsDeleted);
  }
}