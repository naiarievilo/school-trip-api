using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;
using SchoolTripApi.Domain.RatingAggregate.ValueObjects;
using SchoolTripApi.Domain.TripAggregate;
using SchoolTripApi.Domain.TripAggregate.ValueObjects;

namespace SchoolTripApi.Domain.RatingAggregate;

public class Rating : AuditableEntity<RatingId>, IAggregateRoot
{
    public Rating(Guardian guardian, Trip trip, TripRating tripRating, Comment comment, string createdBy)
    {
        Guardian = guardian;
        GuardianId = guardian.Id;
        Trip = trip;
        TripId = trip.Id;
        TripRating = tripRating;
        Comment = comment;

        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    public GuardianId GuardianId { get; private set; }
    public TripId TripId { get; private set; }
    public TripRating TripRating { get; private set; }
    public Comment Comment { get; private set; }

    public Trip Trip { get; private set; }
    public Guardian Guardian { get; private set; }
}