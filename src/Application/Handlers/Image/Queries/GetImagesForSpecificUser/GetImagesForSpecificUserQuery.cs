using Application.Core.Contracts;
using Contracts.Common;
using Contracts.Images;
using Domain.Common.Utilities;

namespace Application.Handlers.Image.Queries.GetImagesForSpecificUser;

public sealed class GetImagesForSpecificUserQuery : IQuery<Result<PagedResponse<ImageResponse>>>
{
    public GetImagesForSpecificUserQuery(Guid userId, int page)
    {
        UserId = userId;
        Page = page;
    }

    public Guid UserId { get; }
    public int Page { get; }
}