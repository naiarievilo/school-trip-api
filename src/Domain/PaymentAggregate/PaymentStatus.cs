namespace SchoolTripApi.Domain.PaymentAggregate;

public enum PaymentStatus
{
    Pending,
    Processing,
    Completed,
    Failed,
    Refunded,
    PartiallyRefunded,
    Disputed,
    Cancelled
}