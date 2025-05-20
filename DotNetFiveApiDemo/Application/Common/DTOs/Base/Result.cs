using System;

namespace DotNetFiveApiDemo.Application.Common.DTOs.Base
{
    public class Result
    {
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error is not null)
                throw new InvalidOperationException("A successful result cannot contain an error message.");

            if (!isSuccess && error is null)
                throw new InvalidOperationException("A failed result must contain an error message.");

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public Error Error { get; }
        public bool IsFailure => !IsSuccess;

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
            return IsSuccess ? onSuccess() : onFailure(Error);
        }

        public Result Ensure(Func<bool> predicate, Error error)
        {
            if (IsFailure) return this;
            return predicate() ? this : Failure(error);
        }

        public static Result<T> Success<T>()
        {
            return new Result<T>(true, default, null);
        }
    }
}