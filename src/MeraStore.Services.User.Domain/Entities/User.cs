namespace MeraStore.Services.User.Domain.Entities;

public class User : SoftDeletableEntity
{
  public string FirstName { get; private set; }
  public string LastName { get; private set; }
  public string UserName { get; private set; }
  public string Email { get; private set; }
  public string? PhoneNumber { get; private set; }
  public bool IsActive { get; private set; } = true;
  public DateTime? LastLoginAt { get; private set; }

  public string FullName => $"{FirstName} {LastName}".Trim();

  private User() { }

  public User(string firstName, string lastName, string userName, string email, string? phoneNumber)
  {
    Id = Ulid.NewUlid().ToString();
    FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
    LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
    UserName = userName ?? throw new ArgumentNullException(nameof(userName));
    Email = email ?? throw new ArgumentNullException(nameof(email));
    PhoneNumber = phoneNumber;
  }

  public void Deactivate() => IsActive = false;
  public void MarkLogin(DateTime timestamp) => LastLoginAt = timestamp;
  public void Delete() => SoftDelete();
}