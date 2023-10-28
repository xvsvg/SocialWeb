using Application.Handlers.Errors;
using Application.Handlers.Extensions;
using FluentValidation;

namespace Application.Handlers.User.Commands.CreateUser;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .WithError(ValidationErrors.Login.NameIsRequired);

        RuleFor(x => x.Password).NotEmpty()
            .WithError(ValidationErrors.Login.PasswordIsRequired);
    }
}