using Application.Core.Contracts;
using Contracts.Friendship;
using Domain.Common.Utilities;

namespace Application.Handlers.Friendship.Commands;

public sealed class RemoveFriendshipCommand : ICommand<Result<FriendshipResponse>>
{
    public RemoveFriendshipCommand(Guid userId, Guid friendId)
    {
        UserId = userId;
        FriendId = friendId;
    }

    public Guid UserId { get; }
    public Guid FriendId { get; }
}