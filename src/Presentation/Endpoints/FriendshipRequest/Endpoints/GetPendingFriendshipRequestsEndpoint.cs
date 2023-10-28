using Application.Handlers.FriendshipRequest.Queries.GetPendingFriendshipRequests;
using Contracts.Common;
using Contracts.FriendshipRequest;
using DataAccess.Contracts;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Presentation.Endpoints.FriendshipRequest.Contracts;

namespace Presentation.Endpoints.FriendshipRequest.Endpoints;

public sealed class GetPendingFriendshipRequestsEndpoint 
    : Endpoint<PendingFriendshipRequestsQuery, PagedResponse<FriendshipRequestResponse>>
{
    private readonly IMediator _mediator;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public GetPendingFriendshipRequestsEndpoint(IUserIdentifierProvider userIdentifierProvider, IMediator mediator)
    {
        _userIdentifierProvider = userIdentifierProvider;
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/friendship-request/pending");
        Summary(s =>
        {
            s.Summary = "gets pending friendship requests for current user";
            s.RequestParam(x => x.Page, "desired page of friendship request bunch");
            s.Response<PagedResponse<FriendshipRequestResponse>>(StatusCodes.Status206PartialContent);
            s.Response(StatusCodes.Status204NoContent, "current page has not friendship requests");
            s.Response(StatusCodes.Status401Unauthorized, "you are not logged in to get your sent friendship requests");
        });
    }

    public override async Task HandleAsync(PendingFriendshipRequestsQuery req, CancellationToken ct)
    {
        if (_userIdentifierProvider.UserId.Equals(Guid.Empty))
            await SendUnauthorizedAsync(ct);

        var getPendingFriendshipRequestsQuery = new GetPendingFriendshipRequestsQuery(_userIdentifierProvider.UserId, req.Page);
        var response = await _mediator.Send(getPendingFriendshipRequestsQuery, ct);

        if (response.Bunch.Any())
            await SendAsync(response, StatusCodes.Status206PartialContent, ct);
        else await SendNoContentAsync(ct);
    }
}