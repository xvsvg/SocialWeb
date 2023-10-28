using Application.Handlers.Friendship.Commands;
using Contracts.Friendship;
using DataAccess.Contracts;
using Domain.Common.Utilities;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Presentation.Endpoints.Friendship.Contracts;

namespace Presentation.Endpoints.Friendship.Endpoints;

public sealed class RemoveFriendshipEndpoint : Endpoint<RemoveFriendship, Result<FriendshipResponse>>
{
    private readonly IMediator _mediator;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public RemoveFriendshipEndpoint(IUserIdentifierProvider userIdentifierProvider, IMediator mediator)
    {
        _userIdentifierProvider = userIdentifierProvider;
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("api/friendships/{friendId:guid}");
        Summary(s =>
        {
            s.Summary = "removes friendship for current user";
            s.RequestParam(x => x.FriendId, "id of friend to remove from friends");
            s.Response(200, "friend successfully removed from friend list");
            s.Response(400, "validation error");
            s.Response(401, "you are not logged in");
        });
    }

    public override async Task HandleAsync(RemoveFriendship req, CancellationToken ct)
    {
        if (_userIdentifierProvider.UserId.Equals(Guid.Empty))
            await SendUnauthorizedAsync(ct);

        var removeCommand = new RemoveFriendshipCommand(_userIdentifierProvider.UserId, req.FriendId);
        var response = await _mediator.Send(removeCommand, ct);

        await response.Match(
            _ => SendOkAsync(ct),
            error => SendAsync(error, StatusCodes.Status400BadRequest, ct));
    }
}