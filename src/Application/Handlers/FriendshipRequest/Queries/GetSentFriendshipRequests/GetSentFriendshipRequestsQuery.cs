using Application.Core.Contracts;
using Contracts.Common;
using Contracts.FriendshipRequest;

namespace Application.Handlers.FriendshipRequest.Queries.GetSentFriendshipRequests;

public sealed class GetSentFriendshipRequestsQuery : IQuery<PagedResponse<FriendshipRequestResponse>>
{
    public GetSentFriendshipRequestsQuery(Guid userId, int page)
    {
        UserId = userId;
        Page = page;
    }

    public Guid UserId { get; }
    public int Page { get; }
}