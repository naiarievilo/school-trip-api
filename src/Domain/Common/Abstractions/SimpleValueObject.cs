using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.Errors;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Common.Abstractions;

public abstract class SimpleValueObject<TValueObject, TValue> : ValueObject
{
    private protected SimpleValueObject(TValue value)
    {
        Value = value;
    }

    public TValue Value { get; protected init; }

    protected static Result<TValueObject> TryFrom<TInput>(TInput input, Func<TInput, TValue> converter)
    {
        try
        {
            var value = converter(input);
            var valueObject = CallValueObjectTargetConstructor(value);
            return Result.Success(valueObject);
        }
        catch (ValueObjectException ex)
        {
            return Result.Failure<TValueObject>(ValueObjectError.FailedToConvertToValueObject, ex.Message);
        }
    }

    public static Result<TValueObject> TryFrom(TValue value)
    {
        try
        {
            var valueObject = CallValueObjectTargetConstructor(value);
            return Result.Success(valueObject);
        }
        catch (ValueObjectException ex)
        {
            return Result.Failure<TValueObject>(ValueObjectError.FailedToConvertToValueObject, ex.Message);
        }
    }

    public static TValueObject From(TValue value)
    {
        return CallValueObjectTargetConstructor(value);
    }

    protected static TValueObject From<TInput>(TInput input, Func<TInput, TValue> converter)
    {
        var value = converter(input);
        return CallValueObjectTargetConstructor(value);
    }

    private static TValueObject CallValueObjectTargetConstructor(TValue value)
    {
        var targetConstructor = SimpleValueObjectConstructorFactory.GetValueObjectConstructor<TValueObject, TValue>();
        return targetConstructor(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value!;
    }
}