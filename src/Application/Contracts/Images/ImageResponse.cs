namespace Contracts.Images;

public sealed class ImageResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}