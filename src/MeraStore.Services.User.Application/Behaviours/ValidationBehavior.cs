using FluentValidation;
using MediatR;
using System.Text.Json;

namespace MeraStore.Services.User.Application.Behaviours;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
  : IPipelineBehavior<TRequest, TResponse>
  where TRequest : notnull
{
  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
    if (!validators.Any())
      return await next(cancellationToken);

    var context = new ValidationContext<TRequest>(request);
    var failures = validators
      .Select(v => v.Validate(context))
      .SelectMany(result => result.Errors)
      .Where(f => f != null)
      .ToList();

    if (failures.Any())
    {
      var errorDetails = failures.Select(f => new
      {
        f.PropertyName,
        f.ErrorMessage
      });

      var errorJson = JsonSerializer.Serialize(errorDetails);
      throw new ValidationException($"Validation failed: {errorJson}", failures);
    }

    return await next(cancellationToken);
  }
}