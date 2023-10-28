using Domain.Core.FriendshipRequest;
using Domain.Core.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityTypeConfiguration;

public class FriendshipRequestConfiguration : IEntityTypeConfiguration<FriendshipRequest>
{
    public void Configure(EntityTypeBuilder<FriendshipRequest> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new
        {
            x.UserId,
            x.FriendId
        });

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(friendshipRequest => friendshipRequest.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(friendshipRequest => friendshipRequest.FriendId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(friendshipRequest => friendshipRequest.Accepted).HasDefaultValue(false);
        builder.Property(friendshipRequest => friendshipRequest.Rejected).HasDefaultValue(false);
    }
}