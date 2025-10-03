using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.SchoolAggregate;
using SchoolTripApi.Domain.SchoolGradeAggregate.ValueObjects;
using SchoolTripApi.Domain.StudentAggregate;

namespace SchoolTripApi.Domain.SchoolGradeAggregate;

public sealed class SchoolGrade(string code) : Entity<SchoolGradeId>, IAggregateRoot
{
    private readonly ICollection<School> _schools = new List<School>();
    private readonly ICollection<Student> _students = new List<Student>();

    public string SchoolGradeCode { get; private set; } = code;

    public IEnumerable<Student> Students => _students;
    public IEnumerable<School> Schools => _schools;

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