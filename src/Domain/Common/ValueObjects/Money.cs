using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Enums;

namespace SchoolTripApi.Domain.Common.ValueObjects;

public sealed class Money(Amount amount, Currency currency) : ValueObject
{
    public Amount Amount { get; } = amount;
    public Currency Currency { get; } = currency;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}