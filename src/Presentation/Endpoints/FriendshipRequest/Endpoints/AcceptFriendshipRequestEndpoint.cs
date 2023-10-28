using Application.Handlers.FriendshipRequest.Commands.AcceptFriendshipRequest;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Presentation.Endpoints.FriendshipRequest.Endpoints;

public sealed class AcceptFriendshipRequestEndpoint : EndpointWithoutRequest
{
    private readonly IMediator _mediator;

    public AcceptFriendshipRequestEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("api/friendship-request/{friendshipRequestId:guid}/accept");
        Summary(s =>
        {
            s.Summary = "accepts friendship request";
            s.Description = "accepts friendship request for current user";
            s.Response(StatusCodes.Status200OK, "request accepted successfully");
            s.Response(StatusCodes.Status400BadRequest, "validation error");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("friendshipRequestId");
        var acceptFriendshipRequestCommand = new AcceptFriendshipRequestCommand(id);
        var response = await _mediator.Send(acceptFriendshipRequestCommand, ct);

        await response.Match(
            _ => SendOkAsync(ct),
            error => SendAsync(error, StatusCodes.Status400BadRequest, ct));
    }
}