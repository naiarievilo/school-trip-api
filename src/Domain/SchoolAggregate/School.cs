using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.ValueObjects;
using SchoolTripApi.Domain.GradeLevelAggregate;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Domain.SchoolAggregate.ValueObjects;
using SchoolTripApi.Domain.SchoolTripAggregate;
using SchoolTripApi.Domain.StudentAggregate;

namespace SchoolTripApi.Domain.SchoolAggregate;

public sealed class School : AuditableEntity<SchoolId>, IAggregateRoot
{
    private readonly ICollection<GradeLevel> _grades = new List<GradeLevel>();
    private readonly ICollection<Guardian> _guardians = new List<Guardian>();
    private readonly ICollection<Student> _students = new List<Student>();
    private readonly ICollection<SchoolTrip> _trips = new List<SchoolTrip>();

    public School(SchoolName name, Cnpj cnpj, Address address, PhoneNumber phoneNumber, string createdBy) :
        base(createdBy)
    {
        Name = name;
        Cnpj = cnpj;
        Address = address;
        PhoneNumber = phoneNumber;
    }

    // For EF Core  (cannot use parameterized constructor for EF Core if parameter is owned type)
    private School(SchoolName name, Cnpj cnpj, PhoneNumber phoneNumber, string createdBy) : base(createdBy)
    {
        Name = name;
        Cnpj = cnpj;
        PhoneNumber = phoneNumber;
    }

    public SchoolName Name { get; private set; }
    public Cnpj Cnpj { get; private set; }
    public Address Address { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; }

    public IEnumerable<Guardian> Guardians => _guardians;
    public IEnumerable<Student> Students => _students;
    public IEnumerable<GradeLevel> Grades => _grades;
    public IEnumerable<SchoolTrip> Trips => _trips;

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
}