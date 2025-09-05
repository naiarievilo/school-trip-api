using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Domain.SchoolAggregate.ValueObjects;
using SchoolTripApi.Domain.SchoolGradeAggregate;
using SchoolTripApi.Domain.StudentAggregate;

namespace SchoolTripApi.Domain.SchoolAggregate;

public sealed class School : AuditableEntity<SchoolId>, IAggregateRoot
{
    public School(List<Guardian> guardians, List<Student> students, List<SchoolGrade> schoolGrades, string createdBy)
    {
        Guardians = guardians;
        Students = students;
        SchoolGrades = schoolGrades;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    public List<Guardian> Guardians { get; private set; }
    public List<Student> Students { get; private set; }
    public List<SchoolGrade> SchoolGrades { get; private set; }
}