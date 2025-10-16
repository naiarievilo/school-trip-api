using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.AgreementAggregate.ValueObjects;

public sealed class AgreementTemplateId : IntegerId<AgreementTemplateId>
{
    internal AgreementTemplateId(int value) : base(value)
    {
    }
}