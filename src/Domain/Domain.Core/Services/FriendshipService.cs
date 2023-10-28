using Domain.Common.Errors.Friendship;
using Domain.Common.Exceptions;
using Domain.Common.Utilities;
using Domain.Core.Friendship;
using Domain.Core.User;
using DomainFriendshipRequest = Domain.Core.FriendshipRequest.FriendshipRequest;
using DomainUser = Domain.Core.User.User;
using DomainFriendship = Domain.Core.Friendship.Friendship;

namespace Domain.Core.Services;

public sealed class FriendshipService
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IUserRepository _userRepository;

    public FriendshipService(IUserRepository userRepository, IFriendshipRepository friendshipRepository)
    {
        _userRepository = userRepository;
        _friendshipRepository = friendshipRepository;
    }

    public async Task CreateFriendshipAsync(DomainFriendshipRequest friendshipRequest)
    {
        if (friendshipRequest.Rejected) 
            throw new DomainException(DomainError.FriendshipRequest.AlreadyRejected);

        var user = await _userRepository.GetByIdAsync(friendshipRequest.UserId);

        if (user is null)
            throw new DomainException(DomainError.FriendshipRequest.UserNotFoundFor(friendshipRequest.UserId));

        var friend = await _userRepository.GetByIdAsync(friendshipRequest.FriendId);

        if (friend is null)
            throw new DomainException(DomainError.FriendshipRequest.FriendNotFoundFor(friendshipRequest.FriendId));

        await _friendshipRepository.Insert(new DomainFriendship(user, friend));
        await _friendshipRepository.Insert(new DomainFriendship(friend, user));
    }

    public async Task<Result<DomainFriendship>> RemoveFriendshipAsync(Guid userId, Guid friendId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null) 
            throw new DomainException(DomainError.FriendshipRequest.UserNotFoundFor(userId));

        var friend = await _userRepository.GetByIdAsync(friendId);

        if (friend is null) 
            throw new DomainException(DomainError.FriendshipRequest.FriendNotFoundFor(friendId));

        var alreadyFriends = await _friendshipRepository.CheckIfFriendsAsync(user, friend);

        if (alreadyFriends is false) 
            return DomainError.Friendship.NotFriends;

        var userToFriendRequest = new DomainFriendship(user, friend);
        var friendToUserRequest = new DomainFriendship(friend, user);

        user.RemoveFriendship(userToFriendRequest);

        await _friendshipRepository.Remove(userToFriendRequest);
        await _friendshipRepository.Remove(friendToUserRequest);

        return userToFriendRequest;
    }
}