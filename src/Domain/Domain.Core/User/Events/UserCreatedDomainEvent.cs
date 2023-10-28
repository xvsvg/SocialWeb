using Domain.Common.Events;

namespace Domain.Core.User.Events;

public sealed class UserCreatedDomainEvent : IDomainEvent
{
    internal UserCreatedDomainEvent(User user)
    {
        User = user;
    }

    public User User { get; }
}