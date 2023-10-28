using Domain.Common.Models;

namespace Domain.Common.Errors.User;

public static class DomainError
{
    public static class User
    {
        public static Error NotFound => new("User.NotFound", "The user was not found.");

        public static Error NotFoundFor<TId>(TId id)
        {
            return new Error("User.NotFoundFor", $"The user with id {id} was not found.");
        }
    }
}