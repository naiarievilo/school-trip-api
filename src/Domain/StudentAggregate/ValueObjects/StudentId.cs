using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.StudentAggregate.ValueObjects;

public sealed class StudentId : GuidId<StudentId>
{
    private StudentId(Guid value) : base(value)
    {
    }
}