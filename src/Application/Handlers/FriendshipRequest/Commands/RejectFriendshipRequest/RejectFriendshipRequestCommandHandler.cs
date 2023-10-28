using Application.Core.Contracts;
using Contracts.FriendshipRequest;
using DataAccess.Contracts;
using Domain.Common.Errors.Friendship;
using Domain.Common.Utilities;
using Domain.Core.FriendshipRequest;
using Domain.Core.User;
using DomainFriendshipRequest = Domain.Core.FriendshipRequest.FriendshipRequest;
using DomainUser = Domain.Core.User.User;

namespace Application.Handlers.FriendshipRequest.Commands.RejectFriendshipRequest;

public sealed class RejectFriendshipRequestCommandHandler
    : ICommandHandler<RejectFriendshipRequestCommand, Result<FriendshipRequestResponse>>
{
    private readonly IDatabaseContext _context;
    private readonly IFriendshipRequestRepository _friendshipRequestRepository;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IUserRepository _userRepository;

    public RejectFriendshipRequestCommandHandler(
        IFriendshipRequestRepository friendshipRequestRepository,
        IDatabaseContext context,
        IUserRepository userRepository,
        IUserIdentifierProvider userIdentifierProvider)
    {
        _friendshipRequestRepository = friendshipRequestRepository;
        _context = context;
        _userRepository = userRepository;
        _userIdentifierProvider = userIdentifierProvider;
    }

    public async Task<Result<FriendshipRequestResponse>> Handle(RejectFriendshipRequestCommand request,
        CancellationToken cancellationToken)
    {
        var friendshipRequest =
            await _friendshipRequestRepository.GetByIdAsync(request.FriendshipRequestId);

        if (friendshipRequest is null) 
            return DomainError.FriendshipRequest.NotFoundFor(request.FriendshipRequestId);

        if (friendshipRequest.FriendId != _userIdentifierProvider.UserId)
            return Domain.Common.Errors.Identity.DomainError.GeneralError.InvalidPermissions;

        var user = await _userRepository.GetByIdAsync(friendshipRequest.UserId);

        if (user is null) 
            return DomainError.FriendshipRequest.UserNotFoundFor(friendshipRequest.UserId);

        var rejectResult = friendshipRequest.Reject();

        if (rejectResult.IsFailure) 
            return rejectResult.Error;

        await _context.SaveChangesAsync(cancellationToken);

        return new FriendshipRequestResponse
        {
            Id = request.FriendshipRequestId,
            FriendId = friendshipRequest.FriendId,
            FriendName = user.Name
        };
    }
}