using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

public sealed class GuardianId : GuidId<GuardianId>
{
    private GuardianId(Guid value) : base(value)
    {
    }
}