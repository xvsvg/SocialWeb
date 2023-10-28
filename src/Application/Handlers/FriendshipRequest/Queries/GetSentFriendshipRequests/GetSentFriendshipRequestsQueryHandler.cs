using Application.Core.Configuration;
using Application.Core.Contracts;
using Contracts.Common;
using Contracts.FriendshipRequest;
using DataAccess.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Handlers.FriendshipRequest.Queries.GetSentFriendshipRequests;

public sealed class GetSentFriendshipRequestsQueryHandler 
    : IQueryHandler<GetSentFriendshipRequestsQuery, PagedResponse<FriendshipRequestResponse>>
{
    private readonly PaginationConfiguration _configuration;
    private readonly IDatabaseContext _context;

    public GetSentFriendshipRequestsQueryHandler(
        IDatabaseContext context,
        IOptions<PaginationConfiguration> configuration)
    {
        _context = context;
        _configuration = configuration.Value;
    }

    public async Task<PagedResponse<FriendshipRequestResponse>> Handle(
        GetSentFriendshipRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var friendshipRequestResponsesQuery =
            from friendshipRequest in _context.FriendshipRequests.AsNoTracking()
            join user in _context.Users.AsNoTracking()
                on friendshipRequest.FriendId equals user.Id
            where friendshipRequest.UserId == request.UserId
                  && friendshipRequest.Rejected == false
                  && friendshipRequest.Accepted == false
            select new FriendshipRequestResponse
            {
                Id = friendshipRequest.Id,
                FriendId = friendshipRequest.FriendId,
                FriendName = user.Name
            };

        var bunch = await friendshipRequestResponsesQuery
            .Skip((request.Page - 1) * _configuration.RecordsPerPage)
            .Take(_configuration.RecordsPerPage)
            .ToListAsync(cancellationToken);

        var totalCount = await friendshipRequestResponsesQuery.CountAsync(cancellationToken);
        
        return new PagedResponse<FriendshipRequestResponse>
        {
            Bunch = bunch.AsReadOnly(),
            CurrentPage = request.Page,
            RecordPerPage = _configuration.RecordsPerPage,
            TotalPages = totalCount / _configuration.RecordsPerPage
        };
    }
}