using Domain.Common.Models;

namespace Domain.Common.Exceptions;

public class DomainException : Exception
{
    public DomainException(Error error) : base(error.Message)
    {
        Error = error;
    }

    public Error Error { get; }
}