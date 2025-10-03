using SchoolTripApi.Domain.AgreementAggregate.ValueObjects;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Domain.AgreementAggregate;

public sealed class Agreement : AuditableEntity<AgreementId>, IAggregateRoot
{
    public Agreement(AgreementDocument agreementDocument, Guardian guardian, string createdBy)
    {
        AgreementDocumentId = agreementDocument.Id;
        AgreementDocument = agreementDocument;
        GuardianId = guardian.Id;
        Guardian = guardian;

        CreatedBy = createdBy;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public AgreementDocumentId AgreementDocumentId { get; private set; }
    public GuardianId GuardianId { get; private set; }

    public AgreementDocument AgreementDocument { get; private set; }
    public Guardian Guardian { get; private set; }
}