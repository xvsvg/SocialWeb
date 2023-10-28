using DataAccess.Contracts;
using Domain.Common.Events;
using Domain.Core.Friendship;
using Domain.Core.FriendshipRequest.Events;
using Domain.Core.Services;
using Domain.Core.User;

namespace Application.Handlers.FriendshipRequest.Events.FriendshipRequestAccepted;

public sealed class CreateFriendshipOnFriendshipRequestAcceptedDomainEventHandler
    : IDomainEventHandler<FriendshipRequestAcceptedDomainEvent>
{
    private readonly IDatabaseContext _context;
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IUserRepository _userRepository;

    public CreateFriendshipOnFriendshipRequestAcceptedDomainEventHandler(
        IUserRepository userRepository,
        IFriendshipRepository friendshipRepository,
        IDatabaseContext context)
    {
        _userRepository = userRepository;
        _friendshipRepository = friendshipRepository;
        _context = context;
    }

    public async Task Handle(FriendshipRequestAcceptedDomainEvent notification, CancellationToken cancellationToken)
    {
        var friendshipService = new FriendshipService(_userRepository, _friendshipRepository);

        await friendshipService.CreateFriendshipAsync(notification.FriendshipRequest);

        await _context.SaveChangesAsync(cancellationToken);
    }
}