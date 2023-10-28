namespace Domain.Core.Image;

public interface IImageRepository
{
    Task<Image?> GetByIdAsync(Guid id);

    Task<Image?> GetByNameAsync(string name);

    Task<Image> InsertAsync(Image image);
}