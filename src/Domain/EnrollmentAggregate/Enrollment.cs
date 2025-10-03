using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.EnrollmentAggregate.ValueObjects;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;
using SchoolTripApi.Domain.PaymentAggregate;
using SchoolTripApi.Domain.StudentAggregate;
using SchoolTripApi.Domain.StudentAggregate.ValueObjects;
using SchoolTripApi.Domain.TripAggregate;
using SchoolTripApi.Domain.TripAggregate.ValueObjects;

namespace SchoolTripApi.Domain.EnrollmentAggregate;

public sealed class Enrollment : AuditableEntity<EnrollmentId>, IAggregateRoot
{
    public Enrollment(Trip trip, Guardian guardian, Student student, Payment payment, string createdBy)
    {
        TripId = trip.Id;
        Trip = trip;
        GuardianId = guardian.Id;
        Guardian = guardian;
        StudentId = student.Id;
        Student = student;
        Payment = payment;

        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public TripId TripId { get; private set; }
    public GuardianId GuardianId { get; private set; }
    public StudentId StudentId { get; private set; }
    public EnrollmentStatus Status { get; private set; } = EnrollmentStatus.AwaitingPayment;

    public Trip Trip { get; private set; }
    public Guardian Guardian { get; private set; }
    public Student Student { get; private set; }
    public Payment? Payment { get; private set; }
}