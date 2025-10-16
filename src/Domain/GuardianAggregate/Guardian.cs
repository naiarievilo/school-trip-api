using SchoolTripApi.Domain.AgreementAggregate;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.ValueObjects;
using SchoolTripApi.Domain.EnrollmentAggregate;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;
using SchoolTripApi.Domain.PaymentAggregate;
using SchoolTripApi.Domain.RatingAggregate;
using SchoolTripApi.Domain.SchoolAggregate;
using SchoolTripApi.Domain.StudentAggregate;
using Cpf = SchoolTripApi.Domain.Common.ValueObjects.Cpf;

namespace SchoolTripApi.Domain.GuardianAggregate;

public sealed class Guardian : AuditableEntity<GuardianId>, IAggregateRoot
{
    private readonly ICollection<Agreement> _agreements = new List<Agreement>();
    private readonly ICollection<Enrollment> _enrollments = new List<Enrollment>();
    private readonly ICollection<Payment> _payments = new List<Payment>();
    private readonly ICollection<Rating> _ratings = new List<Rating>();
    private readonly ICollection<School> _schools = new List<School>();
    private readonly ICollection<Student> _students = new List<Student>();

    private Guardian(AccountId accountId, FullName fullName, Cpf? cpf, Rg? rg, Address? address,
        EmergencyContact? emergencyContact, string createdBy) : base(createdBy)
    {
        Id = GuardianId.From(Guid.NewGuid());
        AccountId = accountId;
        FullName = fullName;
        Cpf = cpf;
        Rg = rg;
        Address = address;
        EmergencyContact = emergencyContact;
    }

    public Guardian(AccountId accountId, FullName fullName, string createdBy)
        : this(accountId, fullName, null, null, null, null, createdBy)
    {
    }

    public AccountId AccountId { get; }
    public FullName? FullName { get; }
    public Cpf? Cpf { get; private set; }
    public Rg? Rg { get; private set; }
    public Address? Address { get; private set; }
    public EmergencyContact? EmergencyContact { get; private set; }
    public GuardianStatus Status { get; private set; } = GuardianStatus.Active;

    public IEnumerable<Agreement> Agreements => _agreements;
    public IEnumerable<Enrollment> Enrollments => _enrollments;
    public IEnumerable<Student> Students => _students;
    public IEnumerable<School> Schools => _schools;
    public IEnumerable<Payment> Payments => _payments;
    public IEnumerable<Rating> Ratings => _ratings;

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

    public Result AddRating(Rating rating)
    {
        _ratings.Add(rating);
        return Result.Success();
    }

    public Result RemoveRating(Rating rating)
    {
        _ratings.Remove(rating);
        return Result.Success();
    }

    public Result AddPayment(Payment payment)
    {
        _payments.Add(payment);
        return Result.Success();
    }

    public Result RemovePayment(Payment payment)
    {
        _payments.Remove(payment);
        return Result.Success();
    }

    public Result AddAgreement(Agreement agreement)
    {
        _agreements.Add(agreement);
        return Result.Success();
    }

    public Result RemoveAgreement(Agreement agreement)
    {
        _agreements.Remove(agreement);
        return Result.Success();
    }
}