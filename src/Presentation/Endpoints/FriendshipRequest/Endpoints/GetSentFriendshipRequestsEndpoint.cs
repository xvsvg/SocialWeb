using Application.Handlers.FriendshipRequest.Queries.GetSentFriendshipRequests;
using Contracts.Common;
using Contracts.FriendshipRequest;
using DataAccess.Contracts;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Presentation.Endpoints.FriendshipRequest.Contracts;

namespace Presentation.Endpoints.FriendshipRequest.Endpoints;

public sealed class GetSentFriendshipRequestsEndpoint 
    : Endpoint<SentFriendshipRequestsQuery, PagedResponse<FriendshipRequestResponse>>
{
    private readonly IMediator _mediator;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public GetSentFriendshipRequestsEndpoint(IUserIdentifierProvider userIdentifierProvider, IMediator mediator)
    {
        _userIdentifierProvider = userIdentifierProvider;
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/friendship-request/sent");
        Summary(s =>
        {
            s.Summary = "gets sent friendship requests for current user";
            s.RequestParam(x => x.Page, "desired page of friendship request bunch");
            s.Response<PagedResponse<FriendshipRequestResponse>>(StatusCodes.Status206PartialContent);
            s.Response(StatusCodes.Status204NoContent, "current page has not friendship requests");
            s.Response(StatusCodes.Status401Unauthorized, "you are not logged in to get your sent friendship requests");
        });
    }

    public override async Task HandleAsync(SentFriendshipRequestsQuery req, CancellationToken ct)
    {
        if (_userIdentifierProvider.UserId.Equals(Guid.Empty)) 
            await SendUnauthorizedAsync(ct);

        var getSentFriendshipRequestsQuery = new GetSentFriendshipRequestsQuery(_userIdentifierProvider.UserId, req.Page);
        var response = await _mediator.Send(getSentFriendshipRequestsQuery, ct);

        if (response.Bunch.Any())
            await SendAsync(response, StatusCodes.Status206PartialContent, ct);
        else await SendNoContentAsync(ct);
    }
}