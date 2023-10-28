using Application.Core.Contracts;
using Contracts.Images;
using Contracts.Users;
using DataAccess.Contracts;
using Domain.Core.Friendship;
using Domain.Core.User;
using Microsoft.EntityFrameworkCore;
using DomainUser = Domain.Core.User.User;

namespace Application.Handlers.User.Queries.GetUserByName;

public sealed class GetUserByNameQueryHandler : IQueryHandler<GetUserByNameQuery, UserResponse?>
{
    private readonly IDatabaseContext _context;
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IUserRepository _userRepository;

    public GetUserByNameQueryHandler(
        IUserRepository userRepository,
        IUserIdentifierProvider userIdentifierProvider,
        IFriendshipRepository friendshipRepository,
        IDatabaseContext context)
    {
        _userRepository = userRepository;
        _userIdentifierProvider = userIdentifierProvider;
        _friendshipRepository = friendshipRepository;
        _context = context;
    }

    public async Task<UserResponse?> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByNameAsync(request.Name);

        if (user is null) 
            return null;

        var response = new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Images = user.Gallery.Select(x => new ImageResponse
            {
                Id = x.Id,
                Name = x.Filename
            }).ToList().AsReadOnly()
        };

        var currentUser = await _userRepository.GetByIdAsync(_userIdentifierProvider.UserId);

        if (currentUser is not null)
            response.IsFriend = await _friendshipRepository.CheckIfFriendsAsync(user, currentUser);

        response.NumberOfFriends = await _context.Friendships
            .CountAsync(x => x.UserId.Equals(user.Id), cancellationToken);

        return response;
    }
}