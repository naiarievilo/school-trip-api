using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.ValueObjects;
using SchoolTripApi.Domain.EnrollmentAggregate;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;
using SchoolTripApi.Domain.SchoolAggregate;
using SchoolTripApi.Domain.StudentAggregate;
using Cpf = SchoolTripApi.Domain.Common.ValueObjects.Cpf;

namespace SchoolTripApi.Domain.GuardianAggregate;

public sealed class Guardian : AuditableEntity<GuardianId>, IAggregateRoot
{
    private readonly ICollection<Enrollment> _enrollments = new List<Enrollment>();
    private readonly ICollection<School> _schools = new List<School>();
    private readonly ICollection<Student> _students = new List<Student>();

    private Guardian(AccountId accountId, FullName fullName, Cpf? cpf, Address? address,
        EmergencyContact? emergencyContact, string createdBy)
    {
        Id = GuardianId.From(Guid.NewGuid());
        AccountId = accountId;
        FullName = fullName;
        Cpf = cpf;
        Address = address;
        EmergencyContact = emergencyContact;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    public Guardian(AccountId accountId, FullName fullName, string createdBy)
        : this(accountId, fullName, null, null, null, createdBy)
    {
    }

    public AccountId AccountId { get; }
    public FullName? FullName { get; }
    public Cpf? Cpf { get; private set; }
    public Address? Address { get; private set; }
    public EmergencyContact? EmergencyContact { get; private set; }
    public GuardianStatus Status { get; private set; } = GuardianStatus.Active;

    public IEnumerable<Student> Students => _students;
    public IEnumerable<School> Schools => _schools;
    public IEnumerable<Enrollment> Enrollments => _enrollments;

    public override void UpdateLastModified(string? lastModifiedBy)
    {
        LastModifiedAt = DateTimeOffset.UtcNow;
        LastModifiedBy = lastModifiedBy ?? "Guardian";
    }

    public Result AssignGuardianship(Student student)
    {
        if (student.GuardianId != Id || student.Guardian != this)
            return Result.Failure(
                GuardianError.FailedToAssignGuardianship);

        _students.Add(student);
        return Result.Success();
    }

    public Result RevokeGuardianship(Student student)
    {
        if (student.GuardianId != Id || student.Guardian != this)
            return Result.Failure(GuardianError.FailedToRevokeGuardianship);
        _students.Remove(student);
        return Result.Success();
    }

    public Result AssociateToSchool(School school)
    {
        _schools.Add(school);
        return Result.Success();
    }

    public Result DisassociateFromSchool(School school)
    {
        _schools.Remove(school);
        return Result.Success();
    }

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