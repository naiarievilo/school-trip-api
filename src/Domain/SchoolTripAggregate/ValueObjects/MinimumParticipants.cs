using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.SchoolTripAggregate.ValueObjects;

public sealed class MinimumParticipants : SimpleValueObject<MinimumParticipants, int>,
    ISimpleValueObjectValidator<int>
{
    public static readonly int MinValue = 1;

    internal MinimumParticipants(int value) : base(Validate(value))
    {
    }

    public static int Validate(int value)
    {
        return value >= MinValue
            ? value
            : throw new ValueObjectException($"Trip's minimum participants must be greater than {MinValue}.");
    }
}