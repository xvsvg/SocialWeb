using Contracts.Images;

namespace Contracts.Users;

public sealed class UserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int NumberOfFriends { get; set; }
    public bool IsFriend { get; set; }
    public IReadOnlyCollection<ImageResponse> Images { get; set; } = null!;
}