using Domain.Common.Errors.Friendship;
using Domain.Common.Models;
using Domain.Common.Utilities;
using Domain.Core.FriendshipRequest.Events;
using DomainUser = Domain.Core.User.User;

namespace Domain.Core.FriendshipRequest;

public sealed class FriendshipRequest : AggregateRoot
{
    public FriendshipRequest(DomainUser user, DomainUser friend)
        : base(Guid.NewGuid())
    {
        Ensure.NotNull(user, "The user is required.", nameof(user));
        Ensure.NotEmpty(user.Id, "The user identifier is required.", $"{nameof(user)}{nameof(user.Id)}");
        Ensure.NotNull(friend, "The friend is required.", nameof(friend));
        Ensure.NotEmpty(friend.Id, "The friend identifier is required.", $"{nameof(friend)}{nameof(user.Id)}");

        UserId = user.Id;
        FriendId = friend.Id;
    }

    private FriendshipRequest()
    {
    }

    public Guid UserId { get; set; }
    public Guid FriendId { get; set; }
    public bool Accepted { get; private set; }
    public bool Rejected { get; private set; }

    public Result<FriendshipRequest> Accept()
    {
        if (Accepted)
            return DomainError.FriendshipRequest.AlreadyAccepted;

        if (Rejected)
            return DomainError.FriendshipRequest.AlreadyRejected;

        Accepted = true;
        Raise(new FriendshipRequestAcceptedDomainEvent(this));

        return this;
    }

    public Result<FriendshipRequest> Reject()
    {
        if (Accepted) 
            return DomainError.FriendshipRequest.AlreadyAccepted;

        if (Rejected) 
            return DomainError.FriendshipRequest.AlreadyRejected;

        Rejected = true;
        Raise(new FriendshipRequestRejectedDomainEvent(this));

        return this;
    }
}