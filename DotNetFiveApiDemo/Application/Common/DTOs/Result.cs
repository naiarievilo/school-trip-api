using System;
using DotNetFiveApiDemo.Application.Common.DTOs.Base;

namespace DotNetFiveApiDemo.Application.Common.DTOs
{
    public class Result<T> : Result
    {
        private readonly T _value;

        protected internal Result(bool isSuccess, T value, Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        public T Value
        {
            get
            {
                if (!IsSuccess) throw new InvalidOperationException("Cannot access value of a failed result.");
                return _value;
            }
        }

        public Result<T> Failure(Func<Error> error)
        {
            return new Result<T>(false, default, error());
        }

        public Result<T> Failure(Func<string, Error> error, string errorMessage)
        {
            return new Result<T>(false, default, error(errorMessage));
        }

        public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
        {
            return IsSuccess ? onSuccess(Value) : onFailure(Error);
        }

        public Result<T> Ensure(Func<T, bool> predicate, Error error)
        {
            if (IsFailure) return this;
            return predicate(Value) ? this : Failure<T>(error);
        }

        public Result<TOutput> Bind<TOutput>(Func<T, Result<TOutput>> func)
        {
            return IsSuccess ? func(Value) : Failure<TOutput>(Error);
        }

        public bool TryGetValue(out T value)
        {
            value = IsSuccess ? Value : default;
            return IsSuccess;
        }
    }
}