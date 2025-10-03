using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.TripAggregate.ValueObjects;

public class MaximumTripParticipants : SimpleValueObject<MaximumTripParticipants, int>, ISimpleValueObjectValidator<int>
{
    public static readonly int MinValue = 1;
    public static readonly int MaxValue = int.MaxValue;

    internal MaximumTripParticipants(int value) : base(Validate(value))
    {
    }

    public static int Validate(int value)
    {
        return value >= MinValue && value <= MaxValue
            ? value
            : throw new ValueObjectException($"Maximum trip participants must be between {MinValue} and {MaxValue}.");
    }
}