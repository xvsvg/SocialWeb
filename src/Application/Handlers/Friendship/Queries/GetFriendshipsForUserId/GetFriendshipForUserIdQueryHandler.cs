using Application.Core.Configuration;
using Application.Core.Contracts;
using Contracts.Common;
using Contracts.Friendship;
using DataAccess.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Handlers.Friendship.Queries.GetFriendshipsForUserId;

public sealed class GetFriendshipForUserIdQueryHandler 
    : IQueryHandler<GetFriendshipForUserIdQuery, PagedResponse<FriendshipResponse>>
{
    private readonly PaginationConfiguration _configuration;
    private readonly IDatabaseContext _context;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public GetFriendshipForUserIdQueryHandler(
        IUserIdentifierProvider userIdentifierProvider,
        IDatabaseContext context,
        IOptions<PaginationConfiguration> configuration)
    {
        _userIdentifierProvider = userIdentifierProvider;
        _context = context;
        _configuration = configuration.Value;
    }

    public async Task<PagedResponse<FriendshipResponse>> Handle(GetFriendshipForUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var friendshipResponsesQuery =
            from friendship in _context.Friendships.AsNoTracking()
            join user in _context.Users.AsNoTracking()
                on friendship.UserId equals user.Id
            join friend in _context.Users.AsNoTracking()
                on friendship.FriendId equals friend.Id
            where friendship.UserId == request.UserId
            orderby friend.Name
            select new FriendshipResponse
            {
                UserId = user.Id,
                UserName = user.Name,
                FriendId = friend.Id,
                FriendName = friend.Name
            };

        var totalCount = await friendshipResponsesQuery.CountAsync(cancellationToken);

        List<FriendshipResponse> friendshipResponsesPage = await friendshipResponsesQuery
            .Skip((request.Page - 1) * _configuration.RecordsPerPage)
            .Take(_configuration.RecordsPerPage)
            .ToListAsync(cancellationToken);

        return new PagedResponse<FriendshipResponse>
        {
            Bunch = friendshipResponsesPage,
            RecordPerPage = _configuration.RecordsPerPage,
            CurrentPage = request.Page,
            TotalPages = totalCount / _configuration.RecordsPerPage
        };
    }
}