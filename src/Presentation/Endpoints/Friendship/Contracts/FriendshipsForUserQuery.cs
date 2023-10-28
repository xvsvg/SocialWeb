using System.ComponentModel;

namespace Presentation.Endpoints.Friendship.Contracts;

public class FriendshipsForUserQuery
{
    [DefaultValue(0)]
    public int Page { get; set; }
}