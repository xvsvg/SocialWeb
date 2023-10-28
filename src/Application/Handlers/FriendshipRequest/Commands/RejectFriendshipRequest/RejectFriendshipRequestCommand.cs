using Application.Core.Contracts;
using Contracts.FriendshipRequest;
using Domain.Common.Utilities;

namespace Application.Handlers.FriendshipRequest.Commands.RejectFriendshipRequest;

public sealed class RejectFriendshipRequestCommand : ICommand<Result<FriendshipRequestResponse>>
{
    public RejectFriendshipRequestCommand(Guid friendshipRequestId)
    {
        FriendshipRequestId = friendshipRequestId;
    }

    public Guid FriendshipRequestId { get; }
}