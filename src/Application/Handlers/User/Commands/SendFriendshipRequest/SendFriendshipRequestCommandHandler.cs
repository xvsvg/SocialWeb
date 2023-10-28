using Application.Core.Contracts;
using Contracts.FriendshipRequest;
using DataAccess.Contracts;
using Domain.Common.Errors.Identity;
using Domain.Common.Utilities;
using Domain.Core.Friendship;
using Domain.Core.FriendshipRequest;
using Domain.Core.User;
using DomainUser = Domain.Core.User.User;

namespace Application.Handlers.User.Commands.SendFriendshipRequest;

public sealed class SendFriendshipRequestCommandHandler 
    : ICommandHandler<SendFriendshipRequestCommand, Result<FriendshipRequestResponse>>
{
    private readonly IDatabaseContext _context;
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IFriendshipRequestRepository _friendshipRequestRepository;
    private readonly IUserRepository _userRepository;

    public SendFriendshipRequestCommandHandler(
        IUserRepository userRepository,
        IFriendshipRepository friendshipRepository,
        IFriendshipRequestRepository friendshipRequestRepository,
        IDatabaseContext context)
    {
        _userRepository = userRepository;
        _friendshipRepository = friendshipRepository;
        _friendshipRequestRepository = friendshipRequestRepository;
        _context = context;
    }

    public async Task<Result<FriendshipRequestResponse>> Handle(
        SendFriendshipRequestCommand request,
        CancellationToken cancellationToken)
    {
        if (request.FriendId.Equals(request.UserId)) 
            return DomainError.GeneralError.InvalidPermissions;

        var user = await _userRepository.GetByIdAsync(request.UserId);

        if (user is null) 
            return Domain.Common.Errors.User.DomainError.User.NotFoundFor(request.UserId);

        var friend = await _userRepository.GetByIdAsync(request.FriendId);

        if (friend is null) 
            return Domain.Common.Errors.User.DomainError.User.NotFoundFor(request.UserId);

        var friendshipRequestResult = await user.SendFriendshipRequestAsync(
            friend,
            _friendshipRepository,
            _friendshipRequestRepository);

        if (friendshipRequestResult.IsFailure) 
            return friendshipRequestResult.Error;

        var friendshipRequest = friendshipRequestResult.Success!;

        await _friendshipRequestRepository.Insert(friendshipRequest);
        await _context.SaveChangesAsync(cancellationToken);

        return new FriendshipRequestResponse
        {
            Id = friendshipRequest.Id,
            FriendId = friendshipRequest.FriendId,
            FriendName = friend.Name
        };
    }
}