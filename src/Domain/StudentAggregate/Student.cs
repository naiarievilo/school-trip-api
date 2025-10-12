using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.ValueObjects;
using SchoolTripApi.Domain.EnrollmentAggregate;
using SchoolTripApi.Domain.GradeLevelAggregate;
using SchoolTripApi.Domain.GradeLevelAggregate.ValueObjects;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;
using SchoolTripApi.Domain.SchoolAggregate;
using SchoolTripApi.Domain.SchoolAggregate.ValueObjects;
using SchoolTripApi.Domain.StudentAggregate.ValueObjects;
using Cpf = SchoolTripApi.Domain.Common.ValueObjects.Cpf;

namespace SchoolTripApi.Domain.StudentAggregate;

public sealed class Student : AuditableEntity<StudentId>, IAggregateRoot
{
    private readonly ICollection<Enrollment> _enrollments = new List<Enrollment>();

    public Student(GuardianId guardianId, SchoolId schoolId, GradeLevelId gradeLevelId, FullName fullName, Cpf cpf,
        DateOnly dateOfBirth, Class? gradeClass, string createdBy) : base(createdBy)
    {
        GuardianId = guardianId;
        SchoolId = schoolId;
        GradeLevelId = gradeLevelId;
        FullName = fullName;
        Cpf = cpf;
        DateOfBirth = dateOfBirth;
        GradeClass = gradeClass;
    }

    public GuardianId GuardianId { get; private set; }
    public SchoolId SchoolId { get; private set; }
    public GradeLevelId GradeLevelId { get; private set; }
    public FullName FullName { get; private set; }
    public Cpf Cpf { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public Class? GradeClass { get; private set; }

    public Guardian? Guardian { get; init; }
    public School? School { get; init; }
    public GradeLevel? GradeLevel { get; init; }

    public IEnumerable<Enrollment> Enrollments => _enrollments;

    public Result AddEnrollment(Enrollment enrollment)
    {
        _enrollments.Add(enrollment);
        return Result.Success();
    }

    public Result RemoveEnrollment(Enrollment enrollment)
    {
        _enrollments.Remove(enrollment);
        return Result.Success();
    }
}