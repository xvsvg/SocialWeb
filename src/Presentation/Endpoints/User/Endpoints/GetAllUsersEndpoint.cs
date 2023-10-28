using Application.Handlers.User.Queries.GetAllUsers;
using Contracts.Common;
using Contracts.Users;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Presentation.Endpoints.User.Contracts;

namespace Presentation.Endpoints.User.Endpoints;

public sealed class GetAllUsersEndpoint : Endpoint<PaginationQuery, PagedResponse<UserResponse>>
{
    private readonly IMediator _mediator;

    public GetAllUsersEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/users");
        Summary(s =>
        {
            s.Summary = "list all users endpoint";
            s.Description = "lists all users from desired page with a bunch of 10 by application defaults";
            s.RequestParam(r => r.Page, "desired page");
            s.Response<PagedResponse<UserResponse>>();
            s.Response(204, "current page has no content");
            s.Response(401, "you are not logged in");
        });
    }

    public override async Task HandleAsync(PaginationQuery req, CancellationToken ct)
    {
        var getAllQuery = new GetAllUsersQuery(req.Page);
        var response = await _mediator.Send(getAllQuery, ct);

        if (response.Bunch.Any())
            await SendAsync(response, StatusCodes.Status206PartialContent, ct);
        else await SendNoContentAsync(ct);
    }
}