using DomainUser = Domain.Core.User.User;

namespace Domain.Core.Friendship;

public interface IFriendshipRepository
{
    ValueTask<bool> CheckIfFriendsAsync(DomainUser user, DomainUser friend);

    Task<Friendship> Insert(Friendship friendship);

    Task Remove(Friendship friendship);
}