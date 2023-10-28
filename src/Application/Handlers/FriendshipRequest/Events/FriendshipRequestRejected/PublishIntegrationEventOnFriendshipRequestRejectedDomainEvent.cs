using Domain.Common.Events;
using Domain.Core.FriendshipRequest.Events;

namespace Application.Handlers.FriendshipRequest.Events.FriendshipRequestRejected;

public sealed class PublishIntegrationEventOnFriendshipRequestRejectedDomainEvent
    : IDomainEventHandler<FriendshipRequestRejectedDomainEvent>
{
    // here can be implemented messaging, streaming, background workers awakening etc.
    public async Task Handle(FriendshipRequestRejectedDomainEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}