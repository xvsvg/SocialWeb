using Domain.Common.Utilities;

namespace Domain.Common.Models;

public abstract class Entity : IEquatable<Entity>
{
    protected Entity(Guid id)
        : this()
    {
        Ensure.NotNull(id, "Id should not be null", nameof(id));
        Ensure.NotEmpty(id, "Id should not be empty", nameof(id));

        Id = id;
    }

#pragma warning disable CS8618
    protected Entity()
    {
    }
#pragma warning restore CS8618

    public Guid Id { get; protected set; }

    public bool Equals(Entity? other)
    {
        return Equals((object?)other);
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity entity && Id.Equals(entity.Id);
    }

    public static bool operator ==(Entity left, Entity right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}