using Domain.Common.Events;

namespace Domain.Core.Friendship.Events;

public sealed class FriendshipRemovedDomainEvent : IDomainEvent
{
    internal FriendshipRemovedDomainEvent(Friendship friendship)
    {
        Friendship = friendship;
    }

    public Friendship Friendship { get; }
}