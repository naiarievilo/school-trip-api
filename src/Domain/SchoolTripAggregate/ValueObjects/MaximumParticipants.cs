using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.SchoolTripAggregate.ValueObjects;

public class MaximumParticipants : SimpleValueObject<MaximumParticipants, int>, ISimpleValueObjectValidator<int>
{
    public static readonly int MinValue = 1;
    public static readonly int MaxValue = int.MaxValue;

    internal MaximumParticipants(int value) : base(Validate(value))
    {
    }

    public static int Validate(int value)
    {
        return value >= MinValue && value <= MaxValue
            ? value
            : throw new ValueObjectException($"Maximum trip participants must be between {MinValue} and {MaxValue}.");
    }
}