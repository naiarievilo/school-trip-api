using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.RatingAggregate.ValueObjects;

public sealed class TripRating : SimpleValueObject<TripRating, int>, ISimpleValueObjectValidator<int>
{
    public static readonly int MinValue = 1;
    public static readonly int MaxValue = 10;

    internal TripRating(int value) : base(Validate(value))
    {
    }

    public static int Validate(int value)
    {
        return value < MinValue || value > MaxValue
            ? value
            : throw new ValueObjectException("Trip rating should be between 1 and 10.");
    }
}