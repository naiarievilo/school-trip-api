using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.ValueObjects;
using SchoolTripApi.Domain.EnrollmentAggregate;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;
using SchoolTripApi.Domain.PaymentAggregate.ValueObjects;

namespace SchoolTripApi.Domain.PaymentAggregate;

public sealed class Payment : AuditableEntity<PaymentId>, IAggregateRoot
{
    private readonly ICollection<Enrollment> _enrollments = new List<Enrollment>();

    public Payment(GuardianId guardianId, Money amount, PaymentMethod paymentMethod, TransactionId transactionId,
        string createdBy) : base(createdBy)
    {
        GuardianId = guardianId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        TransactionId = transactionId;
    }

    // For EF Core  (cannot use parameterized constructor for EF Core if parameter is owned type)
    private Payment(GuardianId guardianId, PaymentMethod paymentMethod, TransactionId transactionId,
        string createdBy) : base(createdBy)
    {
        GuardianId = guardianId;
        PaymentMethod = paymentMethod;
        TransactionId = transactionId;
    }

    public GuardianId GuardianId { get; private set; }
    public Money Amount { get; private set; } = null!;
    public PaymentMethod PaymentMethod { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; } = PaymentStatus.Pending;
    public TransactionId TransactionId { get; private set; }

    public Guardian? Guardian { get; init; }
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