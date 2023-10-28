using Domain.Common.Errors.User;
using Domain.Common.Exceptions;
using Domain.Core.Image;
using Domain.Core.User;
using DomainImage = Domain.Core.Image.Image;
using DomainUser = Domain.Core.User.User;

namespace Domain.Core.Services;

public sealed class ImageService
{
    private readonly IImageRepository _imageRepository;
    private readonly IUserRepository _userRepository;

    public ImageService(IUserRepository userRepository, IImageRepository imageRepository)
    {
        _userRepository = userRepository;
        _imageRepository = imageRepository;
    }

    public async Task<DomainImage> UploadImage(Guid userId, DomainImage image)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null) 
            throw new DomainException(DomainError.User.NotFoundFor(userId));

        var addedImage = user.AddImage(image);
        return await _imageRepository.InsertAsync(addedImage);
    }
}