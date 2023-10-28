using Domain.Common.Models;

namespace Domain.Common.Errors.Identity;

public static partial class DomainError
{
    public static class Login
    {
        public static Error DuplicateLogin => new("Login.DuplicateLogin", "The login is already in use.");
    }
}