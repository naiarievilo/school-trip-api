using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.SchoolGradeAggregate.ValueObjects;

public sealed class GradeLevelId : IntegerId<GradeLevelId>
{
    private GradeLevelId(int value) : base(value)
    {
    }
}