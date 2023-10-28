using Application.Core.Contracts;
using Contracts.Images;
using DataAccess.Contracts;
using Domain.Core.Image;
using Domain.Core.Services;
using Domain.Core.User;
using DomainImage = Domain.Core.Image.Image;

namespace Application.Handlers.Image.Commands;

public sealed class UploadImageCommandHandler
    : ICommandHandler<UploadImageCommand, ImageResponse>
{
    private readonly IDatabaseContext _context;
    private readonly IImageRepository _imageRepository;
    private readonly IUserRepository _userRepository;

    public UploadImageCommandHandler(
        IUserRepository userRepository,
        IImageRepository imageRepository,
        IDatabaseContext context)
    {
        _userRepository = userRepository;
        _imageRepository = imageRepository;
        _context = context;
    }

    public async Task<ImageResponse> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        var imageService = new ImageService(_userRepository, _imageRepository);
        var result = await imageService.UploadImage(request.UserId, DomainImage.Create(request.File));

        await _context.SaveChangesAsync(cancellationToken);

        return new ImageResponse
        {
            Id = result.Id,
            Name = result.Filename
        };
    }
}