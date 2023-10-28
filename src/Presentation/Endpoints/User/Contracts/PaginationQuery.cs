using System.ComponentModel;

namespace Presentation.Endpoints.User.Contracts;

public class PaginationQuery
{
    [DefaultValue(0)] 
    public int Page { get; set; }
}