using Application.Handlers.Friendship.Queries.GetFriendshipsForUserId;
using Contracts.Common;
using Contracts.Friendship;
using DataAccess.Contracts;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Presentation.Endpoints.Friendship.Contracts;

namespace Presentation.Endpoints.Friendship.Endpoints;

public sealed class GetFriendshipsForUserEndpoint : Endpoint<FriendshipsForUserQuery>
{
    private readonly IMediator _mediator;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public GetFriendshipsForUserEndpoint(IMediator mediator, IUserIdentifierProvider userIdentifierProvider)
    {
        _mediator = mediator;
        _userIdentifierProvider = userIdentifierProvider;
    }

    public override void Configure()
    {
        Get("api/friendships");
        Summary(s =>
        {
            s.Summary = "gets friendships for current user";
            s.Response<PagedResponse<FriendshipResponse>>(206);
            s.Response(204, "current page has no content");
            s.Response(401, "you are not logged in to get your friendships");
            s.RequestParam(x => x.Page, "desired page of friendship bunch");
        });
    }

    public override async Task HandleAsync(FriendshipsForUserQuery req, CancellationToken ct)
    {
        if (_userIdentifierProvider.UserId.Equals(Guid.Empty)) 
            await SendUnauthorizedAsync(ct);

        var getFriendshipsQuery = new GetFriendshipForUserIdQuery(_userIdentifierProvider.UserId, req.Page);
        var response = await _mediator.Send(getFriendshipsQuery, ct);

        if (response.Bunch.Any())
            await SendAsync(response, StatusCodes.Status206PartialContent, ct);
        else await SendNoContentAsync(ct);
    }
}