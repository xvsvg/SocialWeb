using Domain.Common.Models;

namespace Domain.Common.Errors.Friendship;

public static partial class DomainError
{
    public static class FriendshipRequest
    {
        public static Error NotFound => new("FriendshipRequest.NotFound", "The friendship request was not found.");

        public static Error UserNotFound => new("FriendshipRequest.UserNotFound", "The user was not found.");

        public static Error FriendNotFound => new("FriendshipRequest.FriendNotFound", "The friend was not found.");

        public static Error AlreadyAccepted
            => new("FriendshipRequest.AlreadyAccepted", "The friendship request has already been accepted.");

        public static Error AlreadyRejected
            => new("FriendshipRequest.AlreadyRejected", "The friendship request has already been rejected.");

        public static Error AlreadyFriends
            => new("FriendshipRequest.AlreadyFriends",
                "The friendship request can not be sent because the users are already friends.");

        public static Error PendingFriendshipRequest
            => new("FriendshipRequest.PendingFriendshipRequest",
                "The friendship request can not be sent because there is a pending friendship request.");

        public static Error NotFoundFor<TId>(TId id)
        {
            return new Error("FriendshipRequest.NotFoundFor", $"'The friendship with id {id} was not found.");
        }

        public static Error UserNotFoundFor<TId>(TId id)
        {
            return new Error("FriendshipRequest.UserNotFound", $"The user with id {id} was not found.");
        }

        public static Error FriendNotFoundFor<TId>(TId id)
        {
            return new Error("FriendshipRequest.FriendNotFound", $"The friend with id {id} was not found.");
        }

        public static Error AlreadyFriendsWith<TId>(TId id)
        {
            return new Error("FriendshipRequest.AlreadyFriends",
                $"The friendship request can not be sent because the user is {id} is already a friend.");
        }
    }
}