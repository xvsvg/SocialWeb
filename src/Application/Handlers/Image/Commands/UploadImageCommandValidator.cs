using Application.Handlers.Errors;
using Application.Handlers.Extensions;
using FluentValidation;

namespace Application.Handlers.Image.Commands;

public sealed class UploadImageCommandValidator : AbstractValidator<UploadImageCommand>
{
    public UploadImageCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithError(ValidationErrors.UploadImageRequest.UserIdRequired);

        RuleFor(x => x.File)
            .NotNull()
            .WithError(ValidationErrors.UploadImageRequest.FileIsRequired);
    }
}