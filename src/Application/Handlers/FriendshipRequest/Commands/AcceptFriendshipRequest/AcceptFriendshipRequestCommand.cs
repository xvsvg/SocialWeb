using Application.Core.Contracts;
using Contracts.FriendshipRequest;
using Domain.Common.Utilities;

namespace Application.Handlers.FriendshipRequest.Commands.AcceptFriendshipRequest;

public sealed class AcceptFriendshipRequestCommand : ICommand<Result<FriendshipRequestResponse>>
{
    public AcceptFriendshipRequestCommand(Guid friendshipRequestId)
    {
        FriendshipRequestId = friendshipRequestId;
    }

    public Guid FriendshipRequestId { get; }
}