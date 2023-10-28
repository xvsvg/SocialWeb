using Application.Handlers.Errors;
using Application.Handlers.Extensions;
using FluentValidation;

namespace Application.Handlers.Friendship.Commands;

public sealed class RemoveFriendshipCommandValidator : AbstractValidator<RemoveFriendshipCommand>
{
    public RemoveFriendshipCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithError(ValidationErrors.RemoveFriendship.UserIdIsRequired);

        RuleFor(x => x.FriendId)
            .NotEmpty()
            .WithError(ValidationErrors.RemoveFriendship.FriendIdIsRequired);
    }
}