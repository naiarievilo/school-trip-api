using System;
using DotNetFiveApiDemo.Core.Common.DTOs.Base;

namespace DotNetFiveApiDemo.Core.Common.DTOs
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
                if (!Succeeded) throw new InvalidOperationException("Cannot access value of a failed result.");
                return _value;
            }
        }

        public Result<T> Failure(Func<string, Error> error, string errorMessage)
        {
            return new Result<T>(false, default, error(errorMessage));
        }

        public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
        {
            return Succeeded ? onSuccess(Value) : onFailure(Error);
        }

        public Result<T> Ensure(Func<T, bool> predicate, Error error)
        {
            if (Failed) return this;
            return predicate(Value) ? this : Failure<T>(error);
        }

        public Result<TOutput> Bind<TOutput>(Func<T, Result<TOutput>> func)
        {
            return Succeeded ? func(Value) : Failure<TOutput>(Error);
        }

        public bool TryGetValue(out T value)
        {
            value = Succeeded ? Value : default;
            return Succeeded;
        }
    }
}