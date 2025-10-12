using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.TripAggregate.ValueObjects;

public class SchoolTripId : IntegerId<int>
{
    private SchoolTripId(int value) : base(value)
    {
    }
}