using Domain.Common.Models;

namespace Domain.Common.Errors.Identity;

public static partial class DomainError
{
    public static class GeneralError
    {
        public static Error Unauthorized => new("GeneralError.Unauthorized", "You should login to do this.");

        public static Error InvalidPermissions =>
            new("GeneralError.InvalidPermissions", "You are not allowed to do this.");
    }
}