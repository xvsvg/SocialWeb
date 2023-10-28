using Domain.Common.Utilities;

namespace Domain.Common.Models;

public sealed class Error : ValueObject
{
    public Error(string code, string message)
    {
        Ensure.NotNull(code, "error code should not be null", nameof(code));
        Ensure.NotNull(message, "error message should not be null", nameof(message));

        Code = code;
        Message = message;
    }

    public string Code { get; }
    public string Message { get; }

    internal static Error None => new(string.Empty, string.Empty);

    public static implicit operator string(Error error)
    {
        return error?.Code ?? string.Empty;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
        yield return Message;
    }
}