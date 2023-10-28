using Application.Handlers.Errors;
using Application.Handlers.Extensions;
using FluentValidation;

namespace Application.Handlers.FriendshipRequest.Commands.AcceptFriendshipRequest;

public sealed class AcceptFriendshipRequestCommandValidator : AbstractValidator<AcceptFriendshipRequestCommand>
{
    public AcceptFriendshipRequestCommandValidator()
    {
        RuleFor(x => x.FriendshipRequestId)
            .NotEmpty()
            .WithError(ValidationErrors.AcceptFriendshipRequest.FriendshipRequestIdIsRequired);
    }
}