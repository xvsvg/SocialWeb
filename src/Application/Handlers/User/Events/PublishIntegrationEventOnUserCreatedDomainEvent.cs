using Domain.Common.Events;
using Domain.Core.User.Events;

namespace Application.Handlers.User.Events;

public sealed class PublishIntegrationEventOnUserCreatedDomainEvent
    : IDomainEventHandler<UserCreatedDomainEvent>
{
    // here can be implemented messaging, streaming, background workers awakening etc.
    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}