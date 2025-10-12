using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.AgreementAggregate.ValueObjects;

public sealed class AgreementDocumentId : IntegerId<AgreementDocumentId>
{
    private AgreementDocumentId(int value) : base(value)
    {
    }
}