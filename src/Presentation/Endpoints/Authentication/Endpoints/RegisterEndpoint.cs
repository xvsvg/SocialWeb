using Application.Handlers.User.Commands.CreateUser;
using Contracts.Authentication;
using DataAccess.Contracts;
using Domain.Common.Errors.Identity;
using Domain.Common.Utilities;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Presentation.Endpoints.Authentication.Contracts;
using Presentation.Endpoints.User.Endpoints;

namespace Presentation.Endpoints.Authentication.Endpoints;

public class RegisterEndpoint : Endpoint<AuthCommand, Result<TokenResponse>>
{
    private readonly IMediator _mediator;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public RegisterEndpoint(IMediator mediator, IUserIdentifierProvider userIdentifierProvider)
    {
        _mediator = mediator;
        _userIdentifierProvider = userIdentifierProvider;
    }

    public override void Configure()
    {
        Post("api/register");
        Summary(s =>
        {
            s.Summary = "register endpoint";
            s.Description = "registers you in system";
            s.RequestParam(r => r.Name, "login");
            s.RequestParam(r => r.Password, "password");
            s.Response<AuthCommand>();
            s.Response<AuthCommand>(400, "invalid request");
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(AuthCommand req, CancellationToken ct)
    {
        if (_userIdentifierProvider.UserId != Guid.Empty)
            await SendAsync(DomainError.GeneralError.InvalidPermissions, StatusCodes.Status400BadRequest, ct);

        var createCommand = new CreateUserCommand(req.Name, req.Password);
        var response = await _mediator.Send(createCommand, ct);

        await response.Match(
            success => SendCreatedAtAsync<GetUserByIdOrNameEndpoint>(new { req.Name }, success,
                generateAbsoluteUrl: true, cancellation: ct),
            error => SendAsync(error, cancellation: ct, statusCode: 400));
    }
}