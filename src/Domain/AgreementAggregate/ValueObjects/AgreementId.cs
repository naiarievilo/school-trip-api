using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.AgreementAggregate.ValueObjects;

public sealed class AgreementId : GuidId<AgreementId>
{
    internal AgreementId(Guid value) : base(value)
    {
    }
}