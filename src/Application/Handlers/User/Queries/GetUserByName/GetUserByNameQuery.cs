using Application.Core.Contracts;
using Contracts.Users;

namespace Application.Handlers.User.Queries.GetUserByName;

public sealed class GetUserByNameQuery : IQuery<UserResponse?>
{
    public GetUserByNameQuery(string name)
    {
        Name = name;
    }

    public string Name { get; }
}