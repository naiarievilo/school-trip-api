using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.ValueObjects;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;
using SchoolTripApi.Domain.SchoolAggregate;
using SchoolTripApi.Domain.StudentAggregate;

namespace SchoolTripApi.Domain.GuardianAggregate;

public sealed class Guardian : AuditableEntity<GuardianId>, IAggregateRoot
{
    private Guardian(AccountId accountId, FullName fullName, Cpf? cpf, Address? address,
        EmergencyContact? emergencyContact, string createdBy, List<Student>? students, List<School>? schools)
    {
        Id = GuardianId.From(Guid.NewGuid());
        AccountId = accountId;
        FullName = fullName;
        Cpf = cpf;
        Address = address;
        EmergencyContact = emergencyContact;
        Students = students ?? [];
        Schools = schools ?? [];
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    public Guardian(AccountId accountId, FullName fullName, string createdBy)
        : this(accountId, fullName, null, null, null, createdBy, null, null)
    {
    }

    public AccountId AccountId { get; }
    public FullName? FullName { get; }
    public Cpf? Cpf { get; private set; }
    public Address? Address { get; private set; }
    public EmergencyContact? EmergencyContact { get; private set; }
    public GuardianStatus Status { get; private set; } = GuardianStatus.Active;
    public List<Student> Students { get; }
    public List<School> Schools { get; private set; }

    public override void UpdateLastModified(string? lastModifiedBy)
    {
        LastModifiedAt = DateTimeOffset.UtcNow;
        LastModifiedBy = lastModifiedBy ?? "Guardian";
    }

    public Result RegisterStudent(Student student)
    {
        Students.Add(student);
        return Result.Success();
    }

    public Result RemoveStudent(Student student)
    {
        Students.Remove(student);
        return Result.Success();
    }
}