using Application.Handlers.Errors;
using Application.Handlers.Extensions;
using FluentValidation;

namespace Application.Handlers.Authentication.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .WithError(ValidationErrors.Login.NameIsRequired);

        RuleFor(x => x.Password).NotEmpty()
            .WithError(ValidationErrors.Login.PasswordIsRequired);
    }
}