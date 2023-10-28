using Domain.Common.Events;
using Domain.Core.FriendshipRequest.Events;

namespace Application.Handlers.FriendshipRequest.Events.FriendshipRequestSent;

public sealed class PublishIntegrationEventOnFriendshipRequestSentDomainEventHandler
    : IDomainEventHandler<FriendshipRequestSentDomainEvent>
{
    // here can be implemented messaging, streaming, background workers awakening etc.
    public async Task Handle(FriendshipRequestSentDomainEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}