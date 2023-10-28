using Domain.Common.Events;

namespace Domain.Core.FriendshipRequest.Events;

public sealed class FriendshipRequestSentDomainEvent : IDomainEvent
{
    internal FriendshipRequestSentDomainEvent(FriendshipRequest friendshipRequest)
    {
        FriendshipRequest = friendshipRequest;
    }

    public FriendshipRequest FriendshipRequest { get; }
}