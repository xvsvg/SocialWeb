using Application.Handlers.Errors;
using Application.Handlers.Extensions;
using FluentValidation;

namespace Application.Handlers.FriendshipRequest.Commands.RejectFriendshipRequest;

public sealed class RejectFriendshipRequestCommandValidator : AbstractValidator<RejectFriendshipRequestCommand>
{
    public RejectFriendshipRequestCommandValidator()
    {
        RuleFor(x => x.FriendshipRequestId)
            .NotEmpty()
            .WithError(ValidationErrors.RejectFriendshipRequest.FriendshipRequestIdIsRequired);
    }
}