using SchoolTripApi.Domain.AgreementAggregate;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.ValueObjects;
using SchoolTripApi.Domain.RatingAggregate;
using SchoolTripApi.Domain.SchoolGradeAggregate;
using SchoolTripApi.Domain.TripAggregate.ValueObjects;

namespace SchoolTripApi.Domain.TripAggregate;

public sealed class Trip : AuditableEntity<TripId>, IAggregateRoot
{
    private readonly ICollection<Agreement> _agreements = new List<Agreement>();
    private readonly ICollection<Rating> _ratings = new List<Rating>();
    private readonly ICollection<SchoolGrade> _schoolGrades = new List<SchoolGrade>();

    public Trip(TripName name, TripCategory category, TripSummary summary, Money price,
        ParticipantsCapacity participantsCapacity, DateTimeOffset saleStartsAt, DateTimeOffset saleEndsAt,
        DateTimeOffset departureAt, DateTimeOffset returnAt, Address departureAddress,
        TripStatus? status, string createdBy)
    {
        Name = name;
        Category = category;
        Summary = summary;
        Price = price;
        ParticipantsCapacity = participantsCapacity;
        Status = status ?? TripStatus.OpenForSale;
        SaleStartsAt = saleStartsAt;
        SaleEndsAt = saleEndsAt;
        DepartureAddress = departureAddress;
        DepartureAt = departureAt;
        ReturnAt = returnAt;

        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    public TripName Name { get; private set; }
    public TripCategory Category { get; private set; }
    public TripSummary Summary { get; private set; }
    public ParticipantsCapacity ParticipantsCapacity { get; private set; }
    public TripStatus Status { get; private set; }
    public Money Price { get; private set; }
    public DateTimeOffset SaleStartsAt { get; private set; }
    public DateTimeOffset SaleEndsAt { get; private set; }
    public Address DepartureAddress { get; private set; }
    public DateTimeOffset DepartureAt { get; private set; }
    public DateTimeOffset ReturnAt { get; private set; }

    public IEnumerable<Rating> Ratings => _ratings;
    public IEnumerable<SchoolGrade> SchoolGrades => _schoolGrades;
    public IEnumerable<Agreement> Agreements => _agreements;

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

    public Result AddRating(Rating rating)
    {
        _ratings.Add(rating);
        return Result.Success();
    }

    public Result RemoveRating(Rating rating)
    {
        _ratings.Remove(rating);
        return Result.Success();
    }

    public Result AddSchoolGrade(SchoolGrade grade)
    {
        _schoolGrades.Add(grade);
        return Result.Success();
    }

    public Result RemoveSchoolGrade(SchoolGrade grade)
    {
        _schoolGrades.Remove(grade);
        return Result.Success();
    }

    public Result AddSchoolGrades(IEnumerable<SchoolGrade> grades)
    {
        foreach (var grade in grades) _schoolGrades.Add(grade);

        return Result.Success();
    }

    public Result RemoveSchoolGrades(IEnumerable<SchoolGrade> grades)
    {
        foreach (var grade in grades) _schoolGrades.Remove(grade);

        return Result.Success();
    }
}