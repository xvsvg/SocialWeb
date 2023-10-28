using Application.Core.Contracts;
using Contracts.FriendshipRequest;
using Domain.Common.Utilities;

namespace Application.Handlers.User.Commands.SendFriendshipRequest;

public sealed class SendFriendshipRequestCommand : ICommand<Result<FriendshipRequestResponse>>
{
    public SendFriendshipRequestCommand(Guid userId, Guid friendId)
    {
        UserId = userId;
        FriendId = friendId;
    }

    public Guid UserId { get; }
    public Guid FriendId { get; }
}