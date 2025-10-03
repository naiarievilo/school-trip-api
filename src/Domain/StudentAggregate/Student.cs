using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.ValueObjects;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;
using SchoolTripApi.Domain.SchoolAggregate;
using SchoolTripApi.Domain.SchoolAggregate.ValueObjects;
using SchoolTripApi.Domain.SchoolGradeAggregate;
using SchoolTripApi.Domain.SchoolGradeAggregate.ValueObjects;
using SchoolTripApi.Domain.StudentAggregate.ValueObjects;
using Cpf = SchoolTripApi.Domain.Common.ValueObjects.Cpf;

namespace SchoolTripApi.Domain.StudentAggregate;

public sealed class Student : AuditableEntity<StudentId>, IAggregateRoot
{
    public Student(Guardian guardian, School school, SchoolGrade schoolGrade, FullName fullName, Cpf cpf,
        DateOnly dateOfBirth, GradeClass? gradeClass, string createdBy)
    {
        GuardianId = guardian.Id;
        Guardian = guardian;
        SchoolId = school.Id;
        School = school;
        SchoolGradeId = schoolGrade.Id;
        SchoolGrade = schoolGrade;

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

    public Guardian Guardian { get; private set; }
    public School School { get; private set; }
    public SchoolGrade SchoolGrade { get; private set; }
}