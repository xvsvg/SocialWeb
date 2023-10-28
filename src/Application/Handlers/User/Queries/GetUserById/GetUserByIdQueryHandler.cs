using Application.Core.Contracts;
using Contracts.Images;
using Contracts.Users;
using DataAccess.Contracts;
using Domain.Core.Friendship;
using Domain.Core.User;
using Microsoft.EntityFrameworkCore;
using DomainUser = Domain.Core.User.User;

namespace Application.Handlers.User.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse?>
{
    private readonly IDatabaseContext _context;
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(
        IUserIdentifierProvider userIdentifierProvider,
        IUserRepository userRepository,
        IDatabaseContext context,
        IFriendshipRepository friendshipRepository)
    {
        _userIdentifierProvider = userIdentifierProvider;
        _userRepository = userRepository;
        _context = context;
        _friendshipRepository = friendshipRepository;
    }

    public async Task<UserResponse?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId.Equals(Guid.Empty))
            return null;

        var user = await _userRepository.GetByIdAsync(request.UserId);

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
            .CountAsync(x => x.UserId.Equals(request.UserId), cancellationToken);

        return response;
    }
}