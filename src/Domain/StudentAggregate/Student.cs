using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;
using SchoolTripApi.Domain.SchoolAggregate.ValueObjects;
using SchoolTripApi.Domain.SchoolGradeAggregate;
using SchoolTripApi.Domain.StudentAggregate.ValueObjects;

namespace SchoolTripApi.Domain.StudentAggregate;

public sealed class Student : AuditableEntity<StudentId>, IAggregateRoot
{
    public Student(GuardianId guardianId, SchoolId schoolId, SchoolGradeId schoolGradeId, FullName fullName, Cpf cpf,
        DateOnly dateOfBirth, GradeClass? gradeClass, string createdBy)
    {
        GuardianId = guardianId;
        SchoolId = schoolId;
        SchoolGradeId = schoolGradeId;
        FullName = fullName;
        Cpf = cpf;
        DateOfBirth = dateOfBirth;
        GradeClass = gradeClass;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    public GuardianId GuardianId { get; private set; }
    public SchoolId SchoolId { get; private set; }
    public SchoolGradeId SchoolGradeId { get; private set; }
    public FullName FullName { get; private set; }
    public Cpf Cpf { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public GradeClass? GradeClass { get; private set; }
}