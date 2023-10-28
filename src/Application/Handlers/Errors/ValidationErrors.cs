using Domain.Common.Models;

namespace Application.Handlers.Errors;

internal static class ValidationErrors
{
    internal static class Login
    {
        internal static Error NameIsRequired => new("Login.NameIsRequired", "The name is required.");

        internal static Error PasswordIsRequired => new("Login.PasswordIsRequired", "The password is required.");
    }

    internal static class RejectFriendshipRequest
    {
        internal static Error FriendshipRequestIdIsRequired => new(
            "RejectFriendshipRequest.FriendshipRequestIdIsRequired",
            "The invitation identifier is required.");
    }

    internal static class AcceptFriendshipRequest
    {
        internal static Error FriendshipRequestIdIsRequired => new(
            "AcceptFriendshipRequest.FriendshipRequestIdIsRequired",
            "The invitation identifier is required.");
    }

    internal static class RemoveFriendship
    {
        internal static Error UserIdIsRequired => new(
            "RemoveFriendship.UserIdIsRequired",
            "The user identifier is required.");

        internal static Error FriendIdIsRequired => new(
            "RemoveFriendship.FriendIdIsRequired",
            "The friend identifier is required.");
    }

    internal static class CreateUser
    {
        internal static Error FirstNameIsRequired =>
            new("CreateUser.FirstNameIsRequired", "The first name is required.");

        internal static Error LastNameIsRequired => new("CreateUser.LastNameIsRequired", "The last name is required.");

        internal static Error EmailIsRequired => new("CreateUser.EmailIsRequired", "The email is required.");

        internal static Error PasswordIsRequired => new("CreateUser.PasswordIsRequired", "The password is required.");
    }

    internal static class SendFriendshipRequest
    {
        internal static Error UserIdIsRequired => new(
            "SendFriendshipRequest.UserIdIsRequired",
            "The user identifier is required.");

        internal static Error FriendIdIsRequired => new(
            "SendFriendshipRequest.FriendIdIsRequired",
            "The friend identifier is required.");
    }

    internal static class UploadImageRequest
    {
        internal static Error UserIdRequired => new(
            "UploadImageRequest.UserIdRequired",
            "The user identifier is required.");

        internal static Error FileIsRequired => new(
            "UploadImageRequest.FileIsRequired",
            "File image is required.");
    }
}