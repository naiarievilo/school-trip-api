using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;
using SchoolTripApi.Domain.RatingAggregate.ValueObjects;
using SchoolTripApi.Domain.SchoolTripAggregate;
using SchoolTripApi.Domain.SchoolTripAggregate.ValueObjects;

namespace SchoolTripApi.Domain.RatingAggregate;

public sealed class Rating : AuditableEntity<RatingId>, IAggregateRoot
{
    public Rating(GuardianId guardianId, SchoolTripId schoolTripId, TripRating tripRating, Comment comment,
        string createdBy) : base(createdBy)
    {
        GuardianId = guardianId;
        SchoolTripId = schoolTripId;
        TripRating = tripRating;
        Comment = comment;
    }

    public GuardianId GuardianId { get; private set; }
    public SchoolTripId SchoolTripId { get; private set; }
    public TripRating TripRating { get; private set; }
    public Comment Comment { get; private set; }

    public SchoolTrip? SchoolTrip { get; init; }
    public Guardian? Guardian { get; init; }
}