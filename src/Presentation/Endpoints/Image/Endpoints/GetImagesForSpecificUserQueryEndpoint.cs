using Application.Handlers.Image.Queries.GetImagesForSpecificUser;
using Contracts.Common;
using Contracts.Images;
using Domain.Common.Utilities;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Presentation.Endpoints.Image.Contracts;

namespace Presentation.Endpoints.Image.Endpoints;

public sealed class GetImagesForSpecificUserQueryEndpoint
    : Endpoint<ImagesForSpecificUserQuery, Result<PagedResponse<ImageResponse>>>
{
    private readonly IMediator _mediator;

    public GetImagesForSpecificUserQueryEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/images/users");
        Summary(s =>
        {
            s.Summary = "list all images for specified user";
            s.Response<Result<PagedResponse<ImageResponse>>>(206);
            s.Response(204, "current page has no content");
            s.Response(401, "you are not logged in");
            s.Response(400, "validation error");
        });
    }

    public override async Task HandleAsync(ImagesForSpecificUserQuery req, CancellationToken ct)
    {
        var imagesQuery = new GetImagesForSpecificUserQuery(req.UserId, req.Page);
        var response = await _mediator.Send(imagesQuery, ct);

        await response.Match(
            success => SendContent(success, ct),
            error => SendAsync(error, StatusCodes.Status400BadRequest, ct));
    }

    private async Task SendContent(PagedResponse<ImageResponse> pagedResponse, CancellationToken cancellationToken)
    {
        if (pagedResponse.Bunch.Any())
            await SendAsync(pagedResponse, StatusCodes.Status206PartialContent, cancellationToken);
        else await SendNoContentAsync(cancellationToken);
    }
}