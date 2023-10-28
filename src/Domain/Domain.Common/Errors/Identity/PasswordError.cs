using Domain.Common.Models;

namespace Domain.Common.Errors.Identity;

public static partial class DomainError
{
    public static class Password
    {
        public static Error Incorrect => new("Password.Incorrect", "The password is incorrect.");

        public static Error TooShort => new("Password.TooShort", "The password is too short.");
    }
}