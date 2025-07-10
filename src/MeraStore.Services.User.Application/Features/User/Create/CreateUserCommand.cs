using MediatR;

using MeraStore.Services.User.Application.Dtos;

namespace MeraStore.Services.User.Application.Features.User.Create;

public sealed record CreateUserCommand(
  string FirstName,
  string LastName,
  string UserName,
  string Email,
  string? PhoneNumber
) : IRequest<UserDto>;