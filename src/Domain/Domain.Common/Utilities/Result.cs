using Domain.Common.Models;

namespace Domain.Common.Utilities;

public class Result<TResult>
{
    private Result(Error error)
    {
        IsSuccess = false;
        Error = error;
        Success = default;
    }

    private Result(TResult result)
    {
        IsSuccess = true;
        Success = result;
        Error = Error.None;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public TResult? Success { get; }
    public Error Error { get; }

    public static implicit operator Result<TResult>(Error error)
    {
        return new Result<TResult>(error);
    }

    public static implicit operator Result<TResult>(TResult result)
    {
        return new Result<TResult>(result);
    }

    public TTarget Match<TTarget>(Func<TResult, TTarget> successAction, Func<Error, TTarget> errorAction)
    {
        if (IsSuccess)
            return successAction(Success!);

        return errorAction(Error);
    }
}