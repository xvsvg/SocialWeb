using Application.Handlers.User.Commands.SendFriendshipRequest;
using DataAccess.Contracts;
using FastEndpoints;
using MediatR;
using Presentation.Endpoints.User.Contracts;

namespace Presentation.Endpoints.User.Endpoints;

public sealed class SendFriendshipRequestEndpoint : Endpoint<FriendshipRequestCommand>
{
    private readonly IMediator _mediator;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public SendFriendshipRequestEndpoint(IUserIdentifierProvider userIdentifierProvider, IMediator mediator)
    {
        _userIdentifierProvider = userIdentifierProvider;
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("api/users/send-friendship-request");
        Summary(s =>
        {
            s.Summary = "send friendship request";
            s.Description = "send friendship request to a specified user";
            s.Response(200, "friendship request successfully sent");
            s.Response(401, "you are not logged in to send friendship request");
            s.Response(400, "user with specified id does not exist");
            s.RequestParam(x => x.FriendId, "id of user to send friendship request");
        });
    }

    public override async Task HandleAsync(FriendshipRequestCommand req, CancellationToken ct)
    {
        if (_userIdentifierProvider.UserId.Equals(Guid.Empty)) 
            await SendUnauthorizedAsync(ct);

        var sendFriendshipRequestCommand =
            new SendFriendshipRequestCommand(_userIdentifierProvider.UserId, req.FriendId);
        var response = await _mediator.Send(sendFriendshipRequestCommand, ct);

        await response.Match(
            _ => SendOkAsync(ct),
            error => SendAsync(error, 400, ct));
    }
}