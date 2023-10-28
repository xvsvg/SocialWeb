using Domain.Common.Models;
using Domain.Core.Friendship;
using Domain.Core.FriendshipRequest;
using Domain.Core.Image;
using Domain.Core.User;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contracts;

public interface IDatabaseContext
{
    DbSet<User> Users { get; set; }
    DbSet<Friendship> Friendships { get; set; }
    DbSet<FriendshipRequest> FriendshipRequests { get; set; }
    DbSet<Image> Images { get; set; }

    void InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities)
        where TEntity : Entity;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}