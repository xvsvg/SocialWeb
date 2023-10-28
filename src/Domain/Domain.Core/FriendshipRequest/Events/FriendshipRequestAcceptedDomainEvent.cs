using Domain.Common.Events;

namespace Domain.Core.FriendshipRequest.Events;

public sealed class FriendshipRequestAcceptedDomainEvent : IDomainEvent
{
    internal FriendshipRequestAcceptedDomainEvent(FriendshipRequest friendshipRequest)
    {
        FriendshipRequest = friendshipRequest;
    }

    public FriendshipRequest FriendshipRequest { get; }
}