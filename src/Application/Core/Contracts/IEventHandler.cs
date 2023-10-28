using Domain.Common.Events;
using MediatR;

namespace Application.Core.Contracts;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}