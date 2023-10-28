using Contracts.Images;
using Domain.Core.Image;

namespace Application.Mapping;

public static class ImageMapping
{
    public static ImageResponse ToDto(this Image image)
    {
        return new ImageResponse
        {
            Id = image.Id,
            Name = image.Filename
        };
    }
}