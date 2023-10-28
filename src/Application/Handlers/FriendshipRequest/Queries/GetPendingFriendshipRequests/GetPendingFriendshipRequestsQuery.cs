using Application.Core.Contracts;
using Contracts.Common;
using Contracts.FriendshipRequest;

namespace Application.Handlers.FriendshipRequest.Queries.GetPendingFriendshipRequests;

public sealed class GetPendingFriendshipRequestsQuery : IQuery<PagedResponse<FriendshipRequestResponse>>
{
    public GetPendingFriendshipRequestsQuery(Guid userId, int page)
    {
        UserId = userId;
        Page = page;
    }

    public Guid UserId { get; }
    public int Page { get; }
}