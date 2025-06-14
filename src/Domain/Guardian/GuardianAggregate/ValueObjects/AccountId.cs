using SchoolTripApi.Domain.Common.Abstractions;

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

    public static implicit operator AccountId(string accountId)
    {
        return From(Guid.Parse(accountId));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}