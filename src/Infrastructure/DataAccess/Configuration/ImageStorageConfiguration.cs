namespace Infrastructure.DataAccess.Configuration;

public class ImageStorageConfiguration
{
    public const string SectionKey = nameof(ImageStorageConfiguration);
    public string Path { get; } = string.Empty;
}