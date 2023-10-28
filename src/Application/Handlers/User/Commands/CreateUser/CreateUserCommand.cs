using Application.Core.Contracts;
using Contracts.Authentication;
using Domain.Common.Utilities;

namespace Application.Handlers.User.Commands.CreateUser;

public sealed class CreateUserCommand : ICommand<Result<TokenResponse>>
{
    public CreateUserCommand(string name, string password)
    {
        Name = name;
        Password = password;
    }

    public string Name { get; }
    public string Password { get; }
}