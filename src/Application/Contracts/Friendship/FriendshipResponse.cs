namespace Contracts.Friendship;

public sealed class FriendshipResponse
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public Guid FriendId { get; set; }
    public string FriendName { get; set; } = string.Empty;
}