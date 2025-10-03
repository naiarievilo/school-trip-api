using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.ValueObjects;
using SchoolTripApi.Domain.EnrollmentAggregate;
using SchoolTripApi.Domain.EnrollmentAggregate.ValueObjects;
using SchoolTripApi.Domain.PaymentAggregate.ValueObjects;

namespace SchoolTripApi.Domain.PaymentAggregate;

public sealed class Payment : AuditableEntity<PaymentId>, IAggregateRoot
{
    public Payment(Enrollment enrollment, Money amount, PaymentMethod paymentMethod, string createdBy)
    {
        Amount = amount;
        PaymentMethod = paymentMethod;
        Enrollment = enrollment;
        EnrollmentId = enrollment.Id;

        CreatedBy = createdBy;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public EnrollmentId EnrollmentId { get; private set; }
    public Money Amount { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; } = PaymentStatus.Pending;
    public TransactionId? TransactionId { get; private set; }

    public Enrollment Enrollment { get; private set; }
}