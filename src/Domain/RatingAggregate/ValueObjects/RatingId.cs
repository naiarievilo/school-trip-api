using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.RatingAggregate.ValueObjects;

public sealed class RatingId : IntegerId<RatingId>
{
    private RatingId(int value) : base(value)
    {
    }
}