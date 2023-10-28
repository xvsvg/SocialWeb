using Domain.Common.Errors.Friendship;
using Domain.Common.Models;
using Domain.Common.Utilities;
using Domain.Core.Friendship;
using Domain.Core.Friendship.Events;
using Domain.Core.FriendshipRequest;
using Domain.Core.FriendshipRequest.Events;
using Domain.Core.Services;
using Domain.Core.User.Events;
using DomainFriendship = Domain.Core.Friendship.Friendship;
using DomainFriendshipRequest = Domain.Core.FriendshipRequest.FriendshipRequest;
using DomainImage = Domain.Core.Image.Image;

namespace Domain.Core.User;

public sealed class User : AggregateRoot
{
    private readonly List<DomainImage> _gallery;
    private readonly string _passwordHash;

    private User(Guid id, string name, string password) : base(id)
    {
        Ensure.NotNull(name, "The name is required.", nameof(name));
        Ensure.NotNull(password, "The password is required.", nameof(password));
        Ensure.NotEmpty(name, "The name should not be null.", nameof(name));
        Ensure.NotEmpty(password, "The password should not be null.", nameof(password));

        Name = name;
        _passwordHash = password;
        _gallery = new List<DomainImage>();
    }

#pragma warning disable CS8618
    private User()
    {
        _gallery = new List<DomainImage>();
    }
#pragma warning restore CS8618

    public string Name { get; }
    public IReadOnlyCollection<DomainImage> Gallery => _gallery;

    public static User Create(string name, string password)
    {
        var user = new User(Guid.NewGuid(), name, password);

        user.Raise(new UserCreatedDomainEvent(user));

        return user;
    }

    public async Task<Result<DomainFriendshipRequest>> SendFriendshipRequestAsync(
        User friend,
        IFriendshipRepository friendshipRepository,
        IFriendshipRequestRepository friendshipRequestRepository
    )
    {
        var friendExist = await friendshipRepository.CheckIfFriendsAsync(this, friend);
        if (friendExist)
            return DomainError.FriendshipRequest.AlreadyFriends;

        var friendshipRequestAlreadySent = await friendshipRequestRepository.CheckForPendingRequestAsync(this, friend);
        if (friendshipRequestAlreadySent)
            return DomainError.FriendshipRequest.PendingFriendshipRequest;

        var friendshipRequest = new DomainFriendshipRequest(this, friend);
        Raise(new FriendshipRequestSentDomainEvent(friendshipRequest));

        return friendshipRequest;
    }

    public DomainImage AddImage(DomainImage image)
    {
        Ensure.NotNull(image, "The image is required.", $"{nameof(image)}");

        _gallery.Add(image);

        return image;
    }

    public bool VerifyPasswordHash(string password, IPasswordHashChecker passwordHashChecker)
    {
        return !string.IsNullOrWhiteSpace(password)
               && passwordHashChecker.HashesMatch(_passwordHash, password);
    }

    internal void RemoveFriendship(DomainFriendship friendship)
    {
        Raise(new FriendshipRemovedDomainEvent(friendship));
    }
}