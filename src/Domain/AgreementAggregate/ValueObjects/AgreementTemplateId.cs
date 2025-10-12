using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.AgreementAggregate.ValueObjects;

public sealed class AgreementTemplateId : IntegerId<AgreementTemplateId>
{
    private AgreementTemplateId(int value) : base(value)
    {
    }
}