using MediatR;

using MeraStore.Services.User.Application.Features.User.Create;
using MeraStore.Shared.Kernel.WebApi.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace MeraStore.Services.User.Api.Endpoints;

/// <summary>
/// Defines the minimal API endpoints for user-related operations.
/// </summary>
public class UserEndpoint : IEndpoint
{
  /// <summary>
  /// Maps user-related endpoints to the application route builder.
  /// </summary>
  /// <param name="app">The application route builder.</param>
  public void MapEndpoints(IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("/users").WithTags("Users");

    group.MapPost("/", async (CreateUserCommand command, [FromServices] IMediator sender,
        CancellationToken cancellationToken) =>
      {
        if (sender is null)
          return Results.Problem("Mediator service is unavailable.");

        var result = await sender.Send(command, cancellationToken);
        return Results.Created($"/users/{result.Id}", result);
      })
      .WithName("CreateUser")
      .WithSummary("Creates a new user")
      .WithDescription("Creates a user and returns the newly created user's information.");
  }
}