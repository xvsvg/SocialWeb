using Application.Handlers.Authentication.Commands.Login;
using Contracts.Authentication;
using FastEndpoints;
using MediatR;
using Presentation.Endpoints.Authentication.Contracts;

namespace Presentation.Endpoints.Authentication.Endpoints;

public sealed class LoginEndpoint : Endpoint<AuthCommand, TokenResponse>
{
    private readonly IMediator _mediator;

    public LoginEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("api/login");
        Summary(s =>
        {
            s.Summary = "login endpoint";
            s.Description = "logins you in system";
            s.RequestParam(r => r.Name, "login");
            s.RequestParam(r => r.Password, "password");
            s.Response<AuthCommand>();
            s.Response<AuthCommand>(404, "login was not found");
            s.Response<AuthCommand>(400, "validation error");
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(AuthCommand req, CancellationToken ct)
    {
        var loginCommand = new LoginCommand(req.Name, req.Password);
        var response = await _mediator.Send(loginCommand, ct);

        await response.Match(
            success => SendOkAsync(success, ct),
            error => SendNotFoundAsync(ct));
    }
}