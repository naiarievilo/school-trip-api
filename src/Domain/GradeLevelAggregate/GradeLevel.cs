using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.GradeLevelAggregate.ValueObjects;
using SchoolTripApi.Domain.SchoolAggregate;
using SchoolTripApi.Domain.SchoolTripAggregate;
using SchoolTripApi.Domain.StudentAggregate;

namespace SchoolTripApi.Domain.GradeLevelAggregate;

public sealed class GradeLevel : Entity<GradeLevelId>, IAggregateRoot
{
    private readonly ICollection<School> _schools = new List<School>();
    private readonly ICollection<Student> _students = new List<Student>();
    private readonly ICollection<SchoolTrip> _trips = new List<SchoolTrip>();

    private GradeLevel(GradeLevelId id, string gradeLevelCode)
    {
        Id = id;
        GradeLevelCode = gradeLevelCode;
    }

    public string GradeLevelCode { get; private set; }

    public IEnumerable<Student> Students => _students;
    public IEnumerable<School> Schools => _schools;
    public IEnumerable<SchoolTrip> Trips => _trips;

    public static GradeLevel Create(GradeLevelId id, string gradeLevelCode)
    {
        return new GradeLevel(id, gradeLevelCode);
    }

    public Result AddTrip(SchoolTrip schoolTrip)
    {
        _trips.Add(schoolTrip);
        return Result.Success();
    }

    public Result RemoveTrip(SchoolTrip schoolTrip)
    {
        _trips.Remove(schoolTrip);
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