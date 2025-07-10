using MeraStore.Shared.Kernel.Persistence;
using Microsoft.EntityFrameworkCore;

using MeraStore.Services.User.Persistence.Configurations;

namespace MeraStore.Services.User.Persistence;

/// <summary>
/// Application-specific database context for the User Service.
/// Inherits from <see cref="DbContextBase"/> to integrate shared persistence configurations.
/// </summary>
/// <param name="options">Database context options injected via DI.</param>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContextBase(options)
{
  /// <summary>
  /// Gets or sets the Users table.
  /// </summary>
  public DbSet<Domain.Entities.User> Users { get; set; } = null!;

  /// <summary>
  /// Configures the entity mappings using the Fluent API.
  /// </summary>
  /// <param name="modelBuilder">The model builder.</param>
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Apply User entity configuration
    modelBuilder.ApplyConfiguration(new UserConfiguration());
    modelBuilder.Entity<Domain.Entities.User>()
      .HasQueryFilter(u => !u.IsDeleted);

    // 🧠 Future-ready: Add more entity configurations here
    // modelBuilder.ApplyConfiguration(new AnotherEntityConfiguration());
  }
}