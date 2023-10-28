using Microsoft.AspNetCore.Http;

namespace Presentation.Endpoints.Image.Contracts;

public class UploadImageRequest
{
    public IFormFile Image { get; set; } = null!;
}