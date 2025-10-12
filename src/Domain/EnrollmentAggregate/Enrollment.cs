using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.EnrollmentAggregate.ValueObjects;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;
using SchoolTripApi.Domain.PaymentAggregate;
using SchoolTripApi.Domain.PaymentAggregate.ValueObjects;
using SchoolTripApi.Domain.SchoolTripAggregate;
using SchoolTripApi.Domain.SchoolTripAggregate.ValueObjects;
using SchoolTripApi.Domain.StudentAggregate;
using SchoolTripApi.Domain.StudentAggregate.ValueObjects;

namespace SchoolTripApi.Domain.EnrollmentAggregate;

public sealed class Enrollment : AuditableEntity<EnrollmentId>, IAggregateRoot
{
    public Enrollment(SchoolTripId schoolTripId, GuardianId guardianId, StudentId studentId, PaymentId? paymentId,
        string createdBy) : base(createdBy)
    {
        SchoolTripId = schoolTripId;
        GuardianId = guardianId;
        StudentId = studentId;
        PaymentId = paymentId;
    }

    public SchoolTripId SchoolTripId { get; private set; }
    public GuardianId GuardianId { get; private set; }
    public StudentId StudentId { get; private set; }
    public PaymentId? PaymentId { get; private set; }
    public EnrollmentStatus Status { get; private set; } = EnrollmentStatus.AwaitingPayment;

    public SchoolTrip? SchoolTrip { get; init; }
    public Guardian? Guardian { get; init; }
    public Student? Student { get; init; }
    public Payment? Payment { get; init; }
}