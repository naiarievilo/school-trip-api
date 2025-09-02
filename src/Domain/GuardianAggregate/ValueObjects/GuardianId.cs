using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.Errors;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

public class GuardianId : ValueObject
{
    private GuardianId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static GuardianId From(Guid value)
    {
        return new GuardianId(value);
    }

    public static Result<GuardianId> TryFrom(string? value)
    {
        try
        {
            return Result.Success((GuardianId)value);
        }
        catch (ValueObjectException ex)
        {
            return Result.Failure<GuardianId>(ValueObjectError.FailedToConvertToValueObject, ex.Message);
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator GuardianId(string? value)
    {
        if (Guid.TryParse(value, out var parsedGuardianId)) return From(parsedGuardianId);
        throw new ValueObjectException("Guid provided couldn't be converted to 'AccountId' value object.");
    }
}