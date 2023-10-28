using Domain.Common.Events;

namespace Domain.Core.FriendshipRequest.Events;

public sealed class FriendshipRequestRemovedDomainEvent : IDomainEvent
{
    internal FriendshipRequestRemovedDomainEvent(FriendshipRequest friendshipRequest)
    {
        FriendshipRequest = friendshipRequest;
    }

    public FriendshipRequest FriendshipRequest { get; }
}