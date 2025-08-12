using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.Errors;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

public class AccountId : ValueObject
{
    private AccountId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static AccountId From(Guid value)
    {
        return new AccountId(value);
    }

    public static Result<AccountId> TryFrom(string? value)
    {
        try
        {
            return Result.Success((AccountId)value);
        }
        catch (ValueObjectException ex)
        {
            return Result.Failure<AccountId>(ValueObjectError.FailedToConvertToValueObject, ex.Message);
        }
    }

    public static explicit operator AccountId(string? value)
    {
        if (Guid.TryParse(value, out var parsedAccountId)) return From(parsedAccountId);
        throw new ValueObjectException("Guid provided couldn't be converted to 'AccountId' value object.");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}