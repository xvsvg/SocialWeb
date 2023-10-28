using Domain.Core.User;
using Infrastructure.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await _context.Users
            .Include(x => x.Gallery)
            .FirstOrDefaultAsync(x => x.Id.Equals(userId));
    }

    public async Task<User?> GetByNameAsync(string name)
    {
        return await _context.Users
            .Include(x => x.Gallery)
            .FirstOrDefaultAsync(x => x.Name.Equals(name));
    }

    public Task<IReadOnlySet<User>> GetAllAsync(int count, int page)
    {
        return Task.FromResult<IReadOnlySet<User>>(_context.Users
            .Skip(count * (page - 1))
            .Take(count)
            .ToHashSet());
    }

    public async Task<User> CreateAsync(User user)
    {
        await _context.Users.AddAsync(user);

        return user;
    }

    public async ValueTask<bool> CheckIfExistByName(string name)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Name.Equals(name)) is not null;
    }
}