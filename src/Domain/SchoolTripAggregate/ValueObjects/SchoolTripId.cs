using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.SchoolTripAggregate.ValueObjects;

public sealed class SchoolTripId : IntegerId<SchoolTrip>
{
    private SchoolTripId(int value) : base(value)
    {
    }
}