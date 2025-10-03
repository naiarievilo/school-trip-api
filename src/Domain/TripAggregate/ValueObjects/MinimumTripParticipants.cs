using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.TripAggregate.ValueObjects;

public sealed class MinimumTripParticipants : SimpleValueObject<MinimumTripParticipants, int>,
    ISimpleValueObjectValidator<int>
{
    public static readonly int MinValue = 1;

    internal MinimumTripParticipants(int value) : base(Validate(value))
    {
    }

    public static int Validate(int value)
    {
        return value >= MinValue
            ? value
            : throw new ValueObjectException($"Trip's minimum participants must be greater than {MinValue}.");
    }
}