using Domain.Common.Models;

namespace Domain.Common.Errors.Event;

public static class DomainError
{
    public static class Event
    {
        public static Error NotFound => new("Event.NotFound", "The event has already been cancelled.");

        public static Error Passed => new("Event.Passed", "The event has already passed and cannot be modified.");
    }
}