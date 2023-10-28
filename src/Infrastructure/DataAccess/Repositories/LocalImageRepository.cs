using Domain.Core.Image;
using Infrastructure.DataAccess.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.DataAccess.Repositories;

internal sealed class LocalImageRepository : IImageRepository
{
    private readonly IImageRepository _imageRepository;
    private readonly ImageStorageConfiguration _imageStorageConfiguration;

    public LocalImageRepository(
        IImageRepository imageRepository,
        IOptions<ImageStorageConfiguration> imageStorageConfiguration)
    {
        _imageRepository = imageRepository;
        _imageStorageConfiguration = imageStorageConfiguration.Value;
    }

    public async Task<Image?> GetByIdAsync(Guid id)
    {
        var image = await _imageRepository.GetByIdAsync(id);

        if (image is not null)
        {
            var filePath = Path.Combine(_imageStorageConfiguration.Path, image.Filename);
            var storedImage = File.Open(filePath, FileMode.Open);
            image.File = () => storedImage;
        }

        return image;
    }

    public async Task<Image?> GetByNameAsync(string name)
    {
        var image = await _imageRepository.GetByNameAsync(name);

        if (image is not null)
        {
            var filePath = Path.Combine(_imageStorageConfiguration.Path, image.Filename);
            var storedImage = File.Open(filePath, FileMode.Open);
            image.File = () => storedImage;
        }

        return image;
    }

    public async Task<Image> InsertAsync(Image image)
    {
        await _imageRepository.InsertAsync(image);

        var filePath = Path.Combine(_imageStorageConfiguration.Path, image.Filename);
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        return image;
    }
}