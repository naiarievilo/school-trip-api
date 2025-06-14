#nullable disable

namespace SchoolTripApi.Domain.Common.DTOs;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error is not null)
            throw new InvalidOperationException("A successful result cannot contain an error message.");

        if (!isSuccess && error is null)
            throw new InvalidOperationException("A failed result must contain an error message.");

        Succeeded = isSuccess;
        Error = error;
    }

    public bool Succeeded { get; }
    public Error Error { get; }
    public bool Failed => !Succeeded;

    public static Result Success()
    {
        return new Result(true, null);
    }

    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }

    public static Result<T> Success<T>(T value)
    {
        return new Result<T>(true, value, null);
    }

    public static Result<T> Failure<T>(Error error)
    {
        return new Result<T>(false, default, error);
    }

    public static Result<T> Failure<T>(Func<Error> error)
    {
        return new Result<T>(false, default, error());
    }

    public static Result<T> Failure<T>(Func<string, Error> error, string errorMessage)
    {
        return new Result<T>(false, default, error(errorMessage));
    }

    public TResult Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        return Succeeded ? onSuccess() : onFailure(Error);
    }

    public void Match(Action<Error> onFailure)
    {
        if (Failed) onFailure(Error);
    }

    public Result Ensure(Func<bool> predicate, Error error)
    {
        if (Failed) return this;
        return predicate() ? this : Failure(error);
    }

    public static Result<T> Success<T>()
    {
        return new Result<T>(true, default, null);
    }

    public static Result Failure(Func<Error> error)
    {
        return new Result(false, error());
    }
}