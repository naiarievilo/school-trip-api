using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.RatingAggregate.ValueObjects;

public sealed class RatingId : IntegerId<int>
{
    private RatingId(int value) : base(value)
    {
    }
}