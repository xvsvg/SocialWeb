using DataAccess.Contracts;
using Domain.Common.Models;
using Domain.Core.Friendship;
using Domain.Core.FriendshipRequest;
using Domain.Core.Image;
using Domain.Core.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Context;

public class DatabaseContext : DbContext, IDatabaseContext
{
    private readonly IMediator _mediator;

    public DatabaseContext(DbContextOptions<DatabaseContext> options,
        IMediator mediator
    )
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Friendship> Friendships { get; set; } = null!;
    public DbSet<FriendshipRequest> FriendshipRequests { get; set; } = null!;
    public DbSet<Image> Images { get; set; } = null!;

    public void InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities)
        where TEntity : Entity
    {
        Set<TEntity>().AddRange(entities);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await PublishDomainEvents(cancellationToken);

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IDataAccessMarker).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    private async Task PublishDomainEvents(CancellationToken cancellationToken)
    {
        var aggregateRoots = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(entityEntry => entityEntry.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = aggregateRoots.SelectMany(entityEntry => entityEntry.Entity.DomainEvents).ToList();

        aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

        var tasks = domainEvents.Select(domainEvent => _mediator.Publish(domainEvent, cancellationToken));

        await Task.WhenAll(tasks);
    }
}