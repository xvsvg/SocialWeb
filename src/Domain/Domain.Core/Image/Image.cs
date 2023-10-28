using Domain.Common.Models;
using Domain.Common.Utilities;
using Microsoft.AspNetCore.Http;

namespace Domain.Core.Image;

public sealed class Image : AggregateRoot
{
    private Func<Stream> _file;

    private Image(Guid id, IFormFile file) : base(id)
    {
        Ensure.NotNull(file, "The file is required.", $"{nameof(file)}");

        _file = file.OpenReadStream;
        Filename = file.FileName;
    }

#pragma warning disable CS8618
    private Image()
    {
    }
#pragma warning restore CS8618

    public Func<Stream> File
    {
        get => _file;
        set
        {
            Ensure.NotNull(value, "file invoker should not be null", nameof(value));
            _file = value;
        }
    }

    public string Filename { get; }

    public static Image Create(IFormFile file)
    {
        return new Image(Guid.NewGuid(), file);
    }

    public Task CopyToAsync(Stream stream)
    {
        return File().CopyToAsync(stream);
    }
}