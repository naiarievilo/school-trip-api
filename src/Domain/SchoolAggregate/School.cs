using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.ValueObjects;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Domain.SchoolAggregate.ValueObjects;
using SchoolTripApi.Domain.SchoolGradeAggregate;
using SchoolTripApi.Domain.StudentAggregate;

namespace SchoolTripApi.Domain.SchoolAggregate;

public sealed class School : AuditableEntity<SchoolId>, IAggregateRoot
{
    private readonly ICollection<Guardian> _guardians = new List<Guardian>();
    private readonly ICollection<SchoolGrade> _schoolGrades = new List<SchoolGrade>();
    private readonly ICollection<Student> _students = new List<Student>();

    public School(SchoolName name, Cnpj cnpj, PhoneNumber phoneNumber, string createdBy)
    {
        Name = name;
        Cnpj = cnpj;
        PhoneNumber = phoneNumber;

        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    public SchoolName Name { get; private set; }
    public Cnpj Cnpj { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }

    public IEnumerable<Guardian> Guardians => _guardians;
    public IEnumerable<Student> Students => _students;
    public IEnumerable<SchoolGrade> SchoolGrades => _schoolGrades;

    public Result AddGuardian(Guardian guardian)
    {
        _guardians.Add(guardian);
        return Result.Success();
    }

    public Result RemoveGuardian(Guardian guardian)
    {
        _guardians.Remove(guardian);
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

    public Result AddSchoolGrade(SchoolGrade schoolGrade)
    {
        _schoolGrades.Add(schoolGrade);
        return Result.Success();
    }

    public Result RemoveSchoolGrade(SchoolGrade schoolGrade)
    {
        _schoolGrades.Remove(schoolGrade);
        return Result.Success();
    }
}