using Application.Core.Contracts;
using Contracts.FriendshipRequest;
using DataAccess.Contracts;
using Domain.Common.Errors.Friendship;
using Domain.Common.Utilities;
using Domain.Core.FriendshipRequest;
using Domain.Core.User;
using DomainFriendshipRequest = Domain.Core.FriendshipRequest.FriendshipRequest;
using DomainUser = Domain.Core.User.User;

namespace Application.Handlers.FriendshipRequest.Commands.AcceptFriendshipRequest;

public sealed class AcceptFriendshipRequestCommandHandler
    : ICommandHandler<AcceptFriendshipRequestCommand,Result<FriendshipRequestResponse>>
{
    private readonly IDatabaseContext _context;
    private readonly IFriendshipRequestRepository _friendshipRequestRepository;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IUserRepository _userRepository;

    public AcceptFriendshipRequestCommandHandler(
        IFriendshipRequestRepository friendshipRequestRepository,
        IUserIdentifierProvider userIdentifierProvider,
        IUserRepository userRepository,
        IDatabaseContext context)
    {
        _friendshipRequestRepository = friendshipRequestRepository;
        _userIdentifierProvider = userIdentifierProvider;
        _userRepository = userRepository;
        _context = context;
    }

    public async Task<Result<FriendshipRequestResponse>> Handle(AcceptFriendshipRequestCommand request,
        CancellationToken cancellationToken)
    {
        var friendshipRequest = await _friendshipRequestRepository.GetByIdAsync(request.FriendshipRequestId);

        if (friendshipRequest is null) return DomainError.FriendshipRequest.NotFound;

        if (friendshipRequest.FriendId != _userIdentifierProvider.UserId)
            return Domain.Common.Errors.Identity.DomainError.GeneralError.InvalidPermissions;

        var user = await _userRepository.GetByIdAsync(friendshipRequest.UserId);

        if (user is null) return DomainError.FriendshipRequest.UserNotFoundFor(friendshipRequest.UserId);

        var acceptResult = friendshipRequest.Accept();

        if (acceptResult.IsFailure) return acceptResult.Error;

        await _context.SaveChangesAsync(cancellationToken);

        return new FriendshipRequestResponse
        {
            Id = friendshipRequest.Id,
            FriendId = friendshipRequest.FriendId,
            FriendName = user.Name
        };
    }
}