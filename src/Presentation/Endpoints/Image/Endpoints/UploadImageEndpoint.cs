using Application.Handlers.Image.Commands;
using Contracts.Images;
using DataAccess.Contracts;
using FastEndpoints;
using MediatR;
using Presentation.Endpoints.Image.Contracts;

namespace Presentation.Endpoints.Image.Endpoints;

public sealed class UploadImageEndpoint : Endpoint<UploadImageRequest, ImageResponse>
{
    private readonly IMediator _mediator;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public UploadImageEndpoint(IUserIdentifierProvider userIdentifierProvider, IMediator mediator)
    {
        _userIdentifierProvider = userIdentifierProvider;
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("api/images");
        Summary(s =>
        {
            s.Summary = "upload image for current user";
            s.Response<ImageResponse>();
            s.Response(400, "validation error");
            s.Response(401, "you are not logged in");
        });
        AllowFileUploads();
    }

    public override async Task HandleAsync(UploadImageRequest req, CancellationToken ct)
    {
        var currentUserId = _userIdentifierProvider.UserId;
        if (currentUserId.Equals(Guid.Empty)) 
            await SendUnauthorizedAsync(ct);

        var uploadCommand = new UploadImageCommand(currentUserId, req.Image);
        var response = await _mediator.Send(uploadCommand, ct);

        await SendCreatedAtAsync<GetImagesForSpecificUserQueryEndpoint>(new { currentUserId }, response,
            generateAbsoluteUrl: true, cancellation: ct);
    }
}