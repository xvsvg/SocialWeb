using Application.Handlers.FriendshipRequest.Commands.RejectFriendshipRequest;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Presentation.Endpoints.FriendshipRequest.Endpoints;

public sealed class RejectFriendshipRequestEndpoint : EndpointWithoutRequest
{
    private readonly IMediator _mediator;

    public RejectFriendshipRequestEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("api/friendship-request/{friendshipRequestId:guid}/reject");
        Summary(s =>
        {
            s.Summary = "rejects friendship request";
            s.Description = "rejects friendship request for current user";
            s.Response(StatusCodes.Status200OK, "request rejected successfully");
            s.Response(StatusCodes.Status400BadRequest, "validation error");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("friendshipRequestId");
        var rejectFriendshipRequestCommand = new RejectFriendshipRequestCommand(id);
        var response = await _mediator.Send(rejectFriendshipRequestCommand, ct);

        await response.Match(
            _ => SendOkAsync(ct),
            error => SendAsync(error, StatusCodes.Status400BadRequest, ct));
    }
}