using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.AgreementAggregate.ValueObjects;

public sealed class AgreementId : GuidId<AgreementId>
{
    private AgreementId(Guid value) : base(value)
    {
    }
}