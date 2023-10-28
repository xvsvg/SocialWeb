using Application.Handlers.Errors;
using Application.Handlers.Extensions;
using FluentValidation;

namespace Application.Handlers.User.Commands.SendFriendshipRequest;

public sealed class SendFriendshipRequestCommandValidator : AbstractValidator<SendFriendshipRequestCommand>
{
    public SendFriendshipRequestCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty()
            .WithError(ValidationErrors.SendFriendshipRequest.UserIdIsRequired);

        RuleFor(x => x.FriendId).NotEmpty()
            .WithError(ValidationErrors.SendFriendshipRequest.FriendIdIsRequired);
    }
}