using Application.Core.Contracts;
using Contracts.Images;
using Microsoft.AspNetCore.Http;

namespace Application.Handlers.Image.Commands;

public sealed class UploadImageCommand : ICommand<ImageResponse>
{
    public UploadImageCommand(Guid userId, IFormFile file)
    {
        UserId = userId;
        File = file;
    }

    public Guid UserId { get; }
    public IFormFile File { get; }
}