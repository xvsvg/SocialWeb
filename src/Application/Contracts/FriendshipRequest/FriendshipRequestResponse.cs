namespace Contracts.FriendshipRequest;

public sealed class FriendshipRequestResponse
{
    public Guid Id { get; set; }
    public Guid FriendId { get; set; }
    public string FriendName { get; set; } = string.Empty;
}