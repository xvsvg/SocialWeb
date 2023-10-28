using System.ComponentModel;

namespace Presentation.Endpoints.Image.Contracts;

public class ImagesForSpecificUserQuery
{
    public Guid UserId { get; set; }

    [DefaultValue(0)] 
    public int Page { get; set; }
}