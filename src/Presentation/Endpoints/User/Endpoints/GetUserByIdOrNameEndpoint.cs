using Application.Handlers.User.Queries.GetUserById;
using Application.Handlers.User.Queries.GetUserByName;
using Contracts.Users;
using FastEndpoints;
using MediatR;
using Presentation.Endpoints.User.Contracts;

namespace Presentation.Endpoints.User.Endpoints;

public class GetUserByIdOrNameEndpoint : Endpoint<UserIdOrNameQuery, UserResponse>
{
    private readonly IMediator _mediator;

    public GetUserByIdOrNameEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/users/{idOrName}");
        Summary(s =>
        {
            s.Summary = "get specified user by id or name";
            s.RequestParam(x => x.IdOrName, "id or name of target user");
            s.Response<UserResponse>();
            s.Response(404, "user not found");
            s.Response(401, "you are not logged in");
        });
    }

    public override async Task HandleAsync(UserIdOrNameQuery req, CancellationToken ct)
    {
        UserResponse? response = default;
        if (Guid.TryParse(req.IdOrName, out var id))
        {
            var getUserByIdQuery = new GetUserByIdQuery(id);
            response = await _mediator.Send(getUserByIdQuery, ct);
        }
        else
        {
            var getUserByNameQuery = new GetUserByNameQuery(req.IdOrName);
            response = await _mediator.Send(getUserByNameQuery, ct);
        }

        if (response is not null)
            await SendOkAsync(response, ct);
        else await SendNotFoundAsync(ct);
    }
}