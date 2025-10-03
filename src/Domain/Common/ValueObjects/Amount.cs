using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Common.ValueObjects;

public sealed class Amount : SimpleValueObject<Amount, decimal>, ISimpleValueObjectValidator<decimal>
{
    internal Amount(decimal value) : base(Validate(value))
    {
    }

    public static decimal Validate(decimal value)
    {
        return value > 0
            ? value
            : throw new ValueObjectException("Amount must be greater than zero.");
    }
}