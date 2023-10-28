using Domain.Core.Image;
using Infrastructure.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal sealed class ImageRepository : IImageRepository
{
    private readonly DatabaseContext _context;

    public ImageRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Image?> GetByIdAsync(Guid id)
    {
        return await _context.Images.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<Image?> GetByNameAsync(string name)
    {
        return await _context.Images.FirstOrDefaultAsync(x => x.Filename.Equals(name));
    }

    public async Task<Image> InsertAsync(Image image)
    {
        await _context.Images.AddAsync(image);

        return image;
    }
}