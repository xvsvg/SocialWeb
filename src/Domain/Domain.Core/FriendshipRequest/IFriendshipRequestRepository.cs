using DomainUser = Domain.Core.User.User;

namespace Domain.Core.FriendshipRequest;

public interface IFriendshipRequestRepository
{
    Task<FriendshipRequest?> GetByIdAsync(Guid id);

    ValueTask<bool> CheckForPendingRequestAsync(DomainUser user, DomainUser friend);

    Task Insert(FriendshipRequest request);
}