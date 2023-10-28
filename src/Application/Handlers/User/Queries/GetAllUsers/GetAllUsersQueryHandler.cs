using Application.Core.Configuration;
using Application.Core.Contracts;
using Application.Mapping;
using Contracts.Common;
using Contracts.Images;
using Contracts.Users;
using DataAccess.Contracts;
using Domain.Core.Friendship;
using Domain.Core.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Handlers.User.Queries.GetAllUsers;

public sealed class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, PagedResponse<UserResponse>>
{
    private readonly IDatabaseContext _context;
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IUserIdentifierProvider _identifierProvider;
    private readonly PaginationConfiguration _paginationConfiguration;
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(
        IUserRepository userRepository,
        IFriendshipRepository friendshipRepository,
        IUserIdentifierProvider identifierProvider,
        IDatabaseContext context,
        IOptions<PaginationConfiguration> paginationConfiguration)
    {
        _userRepository = userRepository;
        _friendshipRepository = friendshipRepository;
        _identifierProvider = identifierProvider;
        _context = context;
        _paginationConfiguration = paginationConfiguration.Value;
    }

    public async Task<PagedResponse<UserResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _identifierProvider.UserId;
        var currentUser = await _userRepository.GetByIdAsync(currentUserId);

        var bunch = await _userRepository
            .GetAllAsync(_paginationConfiguration.RecordsPerPage, request.Page);

        var totalRecords = await _context.Users.CountAsync(cancellationToken);

        if (bunch.Count() is 0)
            return new PagedResponse<UserResponse>
            {
                Bunch = Array.Empty<UserResponse>(),
                RecordPerPage = _paginationConfiguration.RecordsPerPage,
                CurrentPage = request.Page,
                TotalPages = totalRecords / _paginationConfiguration.RecordsPerPage
            };

        var results = bunch.Select(async x =>
        {
            var isFriend = currentUser is not null &&
                           await _friendshipRepository.CheckIfFriendsAsync(currentUser, x);

            var numberOfFriends = await _context.Friendships
                .CountAsync(friendship => friendship.UserId.Equals(x.Id), cancellationToken);

            return new UserResponse
            {
                Id = x.Id,
                IsFriend = isFriend,
                Images = isFriend
                    ? x.Gallery.Select(image => image.ToDto()).ToList()
                    : Array.Empty<ImageResponse>().ToList().AsReadOnly(),
                Name = x.Name,
                NumberOfFriends = numberOfFriends
            };
        });

        return new PagedResponse<UserResponse>
        {
            Bunch = await Task.WhenAll(results),
            RecordPerPage = _paginationConfiguration.RecordsPerPage,
            CurrentPage = request.Page,
            TotalPages = totalRecords / _paginationConfiguration.RecordsPerPage
        };
    }
}