using Application.Core.Contracts;
using Contracts.Common;
using Contracts.Users;

namespace Application.Handlers.User.Queries.GetAllUsers;

public sealed class GetAllUsersQuery : IQuery<PagedResponse<UserResponse>>
{
    public GetAllUsersQuery(int page)
    {
        Page = page;
    }

    public int Page { get; }
}