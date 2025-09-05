using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.SchoolAggregate.ValueObjects;

public sealed class SchoolId : GuidId<SchoolId>
{
    private SchoolId(Guid value) : base(value)
    {
        Value = value;
    }
}