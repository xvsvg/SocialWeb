using Domain.Common.Events;

namespace Domain.Core.FriendshipRequest.Events;

public sealed class FriendshipRequestRejectedDomainEvent : IDomainEvent
{
    internal FriendshipRequestRejectedDomainEvent(FriendshipRequest friendshipRequest)
    {
        FriendshipRequest = friendshipRequest;
    }

    public FriendshipRequest FriendshipRequest { get; }
}