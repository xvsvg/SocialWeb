using Domain.Common.Models;

namespace Domain.Common.Errors.Friendship;

public static partial class DomainError
{
    public static class Friendship
    {
        public static Error UserNotFound => new("Friendship.UserNotFoundFor", "The user was not found.");

        public static Error FriendNotFound => new("Friendship.FriendNotFound", "The friend was not found.");

        public static Error NotFriends => new("Friendship.NotFriends", "The specified users are not friends.");

        public static Error UserNotFoundFor<TId>(TId id)
        {
            return new Error("Friendship.UserNotFound", $"The user with id {id} was not found.");
        }

        public static Error FriendNotFoundFor<TId>(TId id)
        {
            return new Error("Friendship.FriendNotFoundFor", $"The friend with id {id} was not found.");
        }
    }
}