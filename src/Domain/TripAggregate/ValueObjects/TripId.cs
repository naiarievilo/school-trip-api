using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.TripAggregate.ValueObjects;

public class TripId : IntegerId<int>
{
    private TripId(int value) : base(value)
    {
    }
}