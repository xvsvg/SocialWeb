using Application.Core.Contracts;
using Contracts.Users;

namespace Application.Handlers.User.Queries.GetUserById;

public sealed class GetUserByIdQuery : IQuery<UserResponse?>
{
    public GetUserByIdQuery(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}