using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.SchoolAggregate;
using SchoolTripApi.Domain.SchoolGradeAggregate.ValueObjects;
using SchoolTripApi.Domain.StudentAggregate;
using SchoolTripApi.Domain.TripAggregate;

namespace SchoolTripApi.Domain.SchoolGradeAggregate;

public sealed class GradeLevel(string code) : Entity<GradeLevelId>, IAggregateRoot
{
    private readonly ICollection<School> _schools = new List<School>();
    private readonly ICollection<Student> _students = new List<Student>();
    private readonly ICollection<Trip> _trips = new List<Trip>();

    public string SchoolGradeCode { get; private set; } = code;

    public IEnumerable<Student> Students => _students;
    public IEnumerable<School> Schools => _schools;
    public IEnumerable<Trip> Trips => _trips;

    public Result AddTrip(Trip trip)
    {
        _trips.Add(trip);
        return Result.Success();
    }

    public Result RemoveTrip(Trip trip)
    {
        _trips.Remove(trip);
        return Result.Success();
    }

    public Result AddStudent(Student student)
    {
        _students.Add(student);
        return Result.Success();
    }

    public Result RemoveStudent(Student student)
    {
        _students.Remove(student);
        return Result.Success();
    }

    public Result AddSchool(School school)
    {
        _schools.Add(school);
        return Result.Success();
    }

    public Result RemoveSchool(School school)
    {
        _schools.Remove(school);
        return Result.Success();
    }
}