using Domain.Core.FriendshipRequest;
using Domain.Core.User;
using Infrastructure.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal sealed class FriendshipRequestRepository : IFriendshipRequestRepository
{
    private readonly DatabaseContext _context;

    public FriendshipRequestRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<FriendshipRequest?> GetByIdAsync(Guid id)
    {
        return await _context.FriendshipRequests.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async ValueTask<bool> CheckForPendingRequestAsync(User user, User friend)
    {
        return await _context.FriendshipRequests.AnyAsync(x =>
            x.UserId.Equals(user.Id)
            && x.FriendId.Equals(friend.Id)
            && x.Rejected == false
            && x.Accepted == false);
    }

    public async Task Insert(FriendshipRequest request)
    {
        await _context.FriendshipRequests.AddAsync(request);
    }
}