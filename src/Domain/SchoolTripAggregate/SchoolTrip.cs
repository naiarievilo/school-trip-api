using SchoolTripApi.Domain.AgreementAggregate;
using SchoolTripApi.Domain.AgreementAggregate.ValueObjects;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.ValueObjects;
using SchoolTripApi.Domain.EnrollmentAggregate;
using SchoolTripApi.Domain.GradeLevelAggregate;
using SchoolTripApi.Domain.RatingAggregate;
using SchoolTripApi.Domain.SchoolAggregate;
using SchoolTripApi.Domain.SchoolAggregate.ValueObjects;
using SchoolTripApi.Domain.SchoolTripAggregate.ValueObjects;

namespace SchoolTripApi.Domain.SchoolTripAggregate;

public sealed class SchoolTrip : AuditableEntity<SchoolTripId>, IAggregateRoot
{
    private readonly ICollection<Agreement> _agreements = new List<Agreement>();
    private readonly ICollection<Enrollment> _enrollments = new List<Enrollment>();
    private readonly ICollection<GradeLevel> _grades = new List<GradeLevel>();
    private readonly ICollection<Rating> _ratings = new List<Rating>();

    public SchoolTrip(SchoolId schoolId, AgreementTemplateId agreementTemplateId, SchoolTripName name,
        SchoolTripCategory category,
        SchoolTripSummary summary, Money price,
        ParticipantsCapacity participantsCapacity, DateTimeOffset saleStartsAt, DateTimeOffset saleEndsAt,
        DateTimeOffset departureAt, DateTimeOffset returnAt, Address departureAddress,
        SchoolTripStatus status, string createdBy) : base(createdBy)
    {
        AgreementTemplateId = agreementTemplateId;
        SchoolId = schoolId;
        Name = name;
        Category = category;
        Summary = summary;
        Price = price;
        ParticipantsCapacity = participantsCapacity;
        Status = status;
        SaleStartsAt = saleStartsAt;
        SaleEndsAt = saleEndsAt;
        DepartureAddress = departureAddress;
        DepartureAt = departureAt;
        ReturnAt = returnAt;
    }

    // For EF Core (cannot use parameterized constructor for EF Core if parameter is owned type)
    private SchoolTrip(SchoolId schoolId, AgreementTemplateId agreementTemplateId, SchoolTripName name,
        SchoolTripCategory category,
        SchoolTripSummary summary, DateTimeOffset saleStartsAt, DateTimeOffset saleEndsAt,
        DateTimeOffset departureAt, DateTimeOffset returnAt, SchoolTripStatus status,
        string createdBy) : base(createdBy)
    {
        AgreementTemplateId = agreementTemplateId;
        SchoolId = schoolId;
        Name = name;
        Category = category;
        Summary = summary;
        Status = status;
        SaleStartsAt = saleStartsAt;
        SaleEndsAt = saleEndsAt;
        DepartureAt = departureAt;
        ReturnAt = returnAt;
    }

    public AgreementTemplateId AgreementTemplateId { get; private set; }
    public SchoolId SchoolId { get; private set; }
    public SchoolTripName Name { get; private set; }
    public SchoolTripCategory Category { get; private set; }
    public SchoolTripSummary Summary { get; private set; }
    public ParticipantsCapacity ParticipantsCapacity { get; private set; } = null!;
    public SchoolTripStatus Status { get; private set; }
    public Money Price { get; private set; } = null!;
    public DateTimeOffset SaleStartsAt { get; private set; }
    public DateTimeOffset SaleEndsAt { get; private set; }
    public Address DepartureAddress { get; private set; } = null!;
    public DateTimeOffset DepartureAt { get; private set; }
    public DateTimeOffset ReturnAt { get; private set; }

    public IEnumerable<Rating> Ratings => _ratings;
    public IEnumerable<GradeLevel> Grades => _grades;
    public IEnumerable<Agreement> Agreements => _agreements;
    public IEnumerable<Enrollment> Enrollments => _enrollments;

    public AgreementTemplate? AgreementTemplate { get; init; }
    public School? School { get; init; }

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

    public Result AddEnrollment(Enrollment enrollment)
    {
        _enrollments.Add(enrollment);
        return Result.Success();
    }

    public Result RemoveEnrollment(Enrollment enrollment)
    {
        _enrollments.Remove(enrollment);
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

    public Result AddGrade(GradeLevel gradeLevel)
    {
        _grades.Add(gradeLevel);
        return Result.Success();
    }

    public Result RemoveGrade(GradeLevel gradeLevel)
    {
        _grades.Remove(gradeLevel);
        return Result.Success();
    }

    public Result AddGradeLevels(IEnumerable<GradeLevel> grades)
    {
        foreach (var grade in grades) _grades.Add(grade);

        return Result.Success();
    }

    public Result RemoveGradeLevels(IEnumerable<GradeLevel> grades)
    {
        foreach (var grade in grades) _grades.Remove(grade);

        return Result.Success();
    }
}