using Domain.Common.Events;
using Domain.Core.Friendship.Events;

namespace Application.Handlers.Friendship.Events;

public sealed class PublishIntegrationEventOnFriendshipRemovedDomainEvent
    : IDomainEventHandler<FriendshipRemovedDomainEvent>
{
    // here can be implemented messaging, streaming, background workers awakening etc.
    public async Task Handle(FriendshipRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}