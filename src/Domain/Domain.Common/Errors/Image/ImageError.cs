using Domain.Common.Models;

namespace Domain.Common.Errors.Image;

public static class DomainError
{
    public static class Image
    {
        public static Error NotFound => new("Image.NotFound", "The image was not found.");

        public static Error NotFoundFor<TId>(TId id)
        {
            return new Error("Image.NotFoundFor", $"The image with id {id} was not found.");
        }
    }
}