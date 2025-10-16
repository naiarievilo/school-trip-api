using SchoolTripApi.Domain.AgreementAggregate.ValueObjects;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.SchoolTripAggregate;

namespace SchoolTripApi.Domain.AgreementAggregate;

public sealed class AgreementTemplate : AuditableEntity<AgreementTemplateId>, IAggregateRoot
{
    private readonly ICollection<Agreement> _agreements = new List<Agreement>();
    private readonly ICollection<SchoolTrip> _schoolTrips = new List<SchoolTrip>();

    private AgreementTemplate(string richTextContent, int version, TemplateType type, string createdBy) :
        base(createdBy)
    {
        RichTextContent = richTextContent;
        Version = version;
        Type = type;
    }

    public string RichTextContent { get; private set; }
    public int Version { get; private set; }
    public TemplateType Type { get; private set; }

    public IEnumerable<SchoolTrip> SchoolTrips => _schoolTrips;
    public IEnumerable<Agreement> Agreements => _agreements;

    public static AgreementTemplate Create(string richTextContent, int version, TemplateType type, string createdBy)
    {
        return new AgreementTemplate(richTextContent, version, type, createdBy);
    }

    public Result AddAgreement(Agreement agreement)
    {
        _agreements.Add(agreement);
        return Result.Success();
    }

    public Result RemoveAgreement(Agreement agreement)
    {
        _agreements.Remove(agreement);
        return Result.Success();
    }

    public Result AddSchoolTrip(SchoolTrip schoolTrip)
    {
        _schoolTrips.Add(schoolTrip);
        return Result.Success();
    }

    public Result RemoveSchoolTrip(SchoolTrip schoolTrip)
    {
        _schoolTrips.Remove(schoolTrip);
        return Result.Success();
    }
}