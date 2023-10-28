using Domain.Core.Friendship;
using Domain.Core.User;
using Infrastructure.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal sealed class FriendshipRepository : IFriendshipRepository
{
    private readonly DatabaseContext _context;

    public FriendshipRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async ValueTask<bool> CheckIfFriendsAsync(User user, User friend)
    {
        return await _context.Friendships.AnyAsync(x =>
            x.UserId.Equals(user.Id) && x.FriendId.Equals(friend.Id));
    }

    public async Task<Friendship> Insert(Friendship friendship)
    {
        await _context.Friendships.AddAsync(friendship);

        return friendship;
    }

    public Task Remove(Friendship friendship)
    {
        _context.Friendships.Remove(friendship);

        return Task.CompletedTask;
    }
}