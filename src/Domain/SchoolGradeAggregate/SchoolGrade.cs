using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.SchoolGradeAggregate;

public sealed class SchoolGrade(string code) : Entity<int>, IAggregateRoot
{
    public string Code { get; private set; } = code;
}