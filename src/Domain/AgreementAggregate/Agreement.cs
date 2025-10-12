using SchoolTripApi.Domain.AgreementAggregate.ValueObjects;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;
using SchoolTripApi.Domain.SchoolTripAggregate;
using SchoolTripApi.Domain.SchoolTripAggregate.ValueObjects;

namespace SchoolTripApi.Domain.AgreementAggregate;

public sealed class Agreement : AuditableEntity<AgreementId>, IAggregateRoot
{
    public Agreement(SchoolTripId schoolTripId, AgreementTemplateId agreementTemplateId, GuardianId guardianId,
        string? fileName, bool isSigned, DateTimeOffset? signedAt, string createdBy) : base(createdBy)
    {
        SchoolTripId = schoolTripId;
        AgreementTemplateId = agreementTemplateId;
        GuardianId = guardianId;
        FileName = fileName;
        IsSigned = isSigned;
        SignedAt = signedAt;
    }

    public SchoolTripId SchoolTripId { get; private set; }
    public AgreementTemplateId AgreementTemplateId { get; private set; }
    public GuardianId GuardianId { get; private set; }
    public bool IsSigned { get; private set; }
    public DateTimeOffset? SignedAt { get; private set; }
    public string? FileName { get; private set; }

    public AgreementTemplate? AgreementTemplate { get; init; }
    public SchoolTrip? SchoolTrip { get; init; }
    public Guardian? Guardian { get; init; }
}