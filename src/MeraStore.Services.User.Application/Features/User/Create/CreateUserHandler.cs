using AutoMapper;

using MediatR;

using MeraStore.Services.User.Application.Dtos;
using MeraStore.Services.User.Application.Repositories;

namespace MeraStore.Services.User.Application.Features.User.Create;

/// <summary>
/// Handles the creation of a new user by processing the <see cref="CreateUserCommand"/>.
/// </summary>
/// <param name="repository">The user repository used to persist the new user.</param>
/// <param name="mapper">The mapper to convert domain entities to DTOs.</param>
public class CreateUserHandler(IUserRepository repository, IMapper mapper)
  : IRequestHandler<CreateUserCommand, UserDto>
{
  /// <summary>
  /// Handles the command to create a new user.
  /// </summary>
  /// <param name="request">The command containing user details.</param>
  /// <param name="cancellationToken">A token to cancel the operation.</param>
  /// <returns>The <see cref="UserDto"/> of the newly created user.</returns>
  public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
  {
    // Optional: Add pre-validation like checking for existing username/email in future

    var user = new Domain.Entities.User(
      request.FirstName,
      request.LastName,
      request.UserName,
      request.Email,
      request.PhoneNumber
    );

    var result = await repository.AddAsync(user, cancellationToken);

    return mapper.Map<UserDto>(result);
  }
}