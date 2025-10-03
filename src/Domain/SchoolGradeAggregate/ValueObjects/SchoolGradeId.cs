using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.SchoolGradeAggregate.ValueObjects;

public sealed class SchoolGradeId : IntegerId<SchoolGradeId>
{
    private SchoolGradeId(int value) : base(value)
    {
    }
}