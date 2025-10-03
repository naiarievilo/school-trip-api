using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.PaymentAggregate.ValueObjects;

public sealed class PaymentId : GuidId<PaymentId>
{
    private PaymentId(Guid value) : base(value)
    {
    }
}