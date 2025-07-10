namespace MeraStore.Services.User.Application.Dtos;
public class UserDto
{
  public string Id { get; init; } = default!;
  public string FirstName { get; init; } = default!;
  public string LastName { get; init; } = default!;
  public string FullName => $"{FirstName} {LastName}";
  public string UserName { get; init; } = default!;
  public string Email { get; init; } = default!;
  public string? PhoneNumber { get; init; }
  public bool IsActive { get; init; }
  public DateTime? LastLoginAt { get; init; }
}
