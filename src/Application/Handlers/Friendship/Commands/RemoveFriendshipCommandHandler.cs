using Application.Core.Contracts;
using Contracts.Friendship;
using DataAccess.Contracts;
using Domain.Common.Errors.Identity;
using Domain.Common.Utilities;
using Domain.Core.Friendship;
using Domain.Core.Services;
using Domain.Core.User;

namespace Application.Handlers.Friendship.Commands;

public sealed class RemoveFriendshipCommandHandler
    : ICommandHandler<RemoveFriendshipCommand, Result<FriendshipResponse>>
{
    private readonly IDatabaseContext _context;
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IUserRepository _userRepository;

    public RemoveFriendshipCommandHandler(
        IUserIdentifierProvider userIdentifierProvider,
        IUserRepository userRepository,
        IFriendshipRepository friendshipRepository,
        IDatabaseContext context)
    {
        _userIdentifierProvider = userIdentifierProvider;
        _userRepository = userRepository;
        _friendshipRepository = friendshipRepository;
        _context = context;
    }

    public async Task<Result<FriendshipResponse>> Handle(RemoveFriendshipCommand request,
        CancellationToken cancellationToken)
    {
        if (request.UserId != _userIdentifierProvider.UserId) 
            return DomainError.GeneralError.InvalidPermissions;

        var friendshipService = new FriendshipService(_userRepository, _friendshipRepository);
        var result = await friendshipService.RemoveFriendshipAsync(request.UserId, request.FriendId);

        if (result.IsFailure) 
            return result.Error;

        await _context.SaveChangesAsync(cancellationToken);

        var user = await _userRepository.GetByIdAsync(request.UserId);
        var friend = await _userRepository.GetByIdAsync(request.FriendId);
        return new FriendshipResponse
        {
            UserId = request.UserId,
            FriendId = request.FriendId,
            UserName = user!.Name,
            FriendName = friend!.Name
        };
    }
}