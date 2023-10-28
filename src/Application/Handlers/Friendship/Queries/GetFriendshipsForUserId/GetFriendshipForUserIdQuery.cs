using Application.Core.Contracts;
using Contracts.Common;
using Contracts.Friendship;

namespace Application.Handlers.Friendship.Queries.GetFriendshipsForUserId;

public sealed class GetFriendshipForUserIdQuery : IQuery<PagedResponse<FriendshipResponse>>
{
    public GetFriendshipForUserIdQuery(Guid userId, int page)
    {
        UserId = userId;
        Page = page;
    }

    public Guid UserId { get; }
    public int Page { get; }
}