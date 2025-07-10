using FluentValidation;

namespace MeraStore.Services.User.Application.Features.User.Create;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
  public CreateUserValidator()
  {
    RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
    RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
    RuleFor(x => x.UserName).NotEmpty().MaximumLength(50);
    RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
    RuleFor(x => x.PhoneNumber).MaximumLength(15).When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
  }
}