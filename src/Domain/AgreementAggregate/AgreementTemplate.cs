using SchoolTripApi.Domain.AgreementAggregate.ValueObjects;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.TripAggregate;
using SchoolTripApi.Domain.TripAggregate.ValueObjects;

namespace SchoolTripApi.Domain.AgreementAggregate;

public sealed class AgreementDocument : AuditableEntity<AgreementDocumentId>, IAggregateRoot
{
    public AgreementDocument(Trip trip, string richTextContent, int version, string createdBy)
    {
        TripId = trip.Id;
        Trip = trip;

        RichTextContent = richTextContent;
        Version = version;

        CreatedBy = createdBy;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public TripId TripId { get; private set; }
    public string RichTextContent { get; private set; }
    public int Version { get; private set; }

    public Trip Trip { get; private set; }
}