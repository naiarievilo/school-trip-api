using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.EnrollmentAggregate.ValueObjects;

public sealed class EnrollmentId : GuidId<Enrollment>
{
    private EnrollmentId(Guid value) : base(value)
    {
    }
}