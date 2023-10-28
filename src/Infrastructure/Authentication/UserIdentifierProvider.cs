using System.Security.Claims;
using DataAccess.Contracts;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authentication;

internal sealed class UserIdentifierProvider : IUserIdentifierProvider
{
    public UserIdentifierProvider(IHttpContextAccessor httpContextAccessor)
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirstValue("userId")
                          ?? string.Empty;

        if (userIdClaim.Equals(string.Empty))
            UserId = Guid.Empty;
        else
            UserId = new Guid(userIdClaim);
    }

    public Guid UserId { get; }
}