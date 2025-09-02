using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.Errors;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Common.Abstractions;

public abstract class StronglyTypedId<TId, TValue> : ValueObject
{
    private protected StronglyTypedId(TValue value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public TValue Value { get; }

    protected abstract TId CreateInstance(TValue value);

    protected static Result<TId> TryFrom<TInput>(TInput value, Func<TInput, Result<TValue>> converter)
    {
        try
        {
            var convertToTValue = converter(value);
            if (convertToTValue.Failed) return Result.Failure<TId>(convertToTValue.Error);

            var stronglyTypedId = (TId)Activator.CreateInstance(typeof(TId), convertToTValue.Value)!;
            return Result.Success(stronglyTypedId);
        }
        catch (ValueObjectException ex)
        {
            return Result.Failure<TId>(ValueObjectError.FailedToConvertToValueObject, ex.Message);
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value!;
    }

    public override string ToString()
    {
        return Value?.ToString() ?? string.Empty;
    }
}