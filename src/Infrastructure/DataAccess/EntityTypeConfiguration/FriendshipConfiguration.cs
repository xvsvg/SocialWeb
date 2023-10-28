using Domain.Core.Friendship;
using Domain.Core.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityTypeConfiguration;

public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
{
    public void Configure(EntityTypeBuilder<Friendship> builder)
    {
        builder.HasKey(friendship => new
        {
            friendship.UserId,
            friendship.FriendId
        });

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(friendship => friendship.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(friendship => friendship.FriendId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Ignore(friendship => friendship.Id);
    }
}