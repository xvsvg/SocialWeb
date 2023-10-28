using Domain.Common.Models;
using Domain.Common.Utilities;
using DomainUser = Domain.Core.User.User;

namespace Domain.Core.Friendship;

public sealed class Friendship : AggregateRoot
{
    public Friendship(DomainUser user, DomainUser friend)
        : base(Guid.NewGuid())
    {
        Ensure.NotNull(user, "The user is required.", nameof(user));
        Ensure.NotEmpty(user.Id, "The user identifier is required.", $"{nameof(user)}{nameof(user.Id)}");
        Ensure.NotNull(friend, "The friend is required.", nameof(friend));
        Ensure.NotEmpty(friend.Id, "The friend identifier is required.", $"{nameof(friend)}{nameof(friend.Id)}");

        UserId = user.Id;
        FriendId = friend.Id;
    }

    private Friendship()
    {
    }

    public Guid UserId { get; }
    public Guid FriendId { get; }
}