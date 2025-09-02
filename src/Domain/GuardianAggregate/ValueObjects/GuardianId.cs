using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

public class GuardianId(Guid value) : GuidId<GuardianId>(value)
{
    protected override GuardianId CreateInstance(Guid value)
    {
        return new GuardianId(value);
    }
}