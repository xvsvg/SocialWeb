using Application.Core.Contracts;
using Contracts.Authentication;
using Domain.Common.Utilities;

namespace Application.Handlers.Authentication.Commands.Login;

public sealed class LoginCommand : ICommand<Result<TokenResponse>>
{
    public LoginCommand(string name, string password)
    {
        Name = name;
        Password = password;
    }

    public string Name { get; }
    public string Password { get; }
}