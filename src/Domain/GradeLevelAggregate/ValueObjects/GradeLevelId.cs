using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.GradeLevelAggregate.ValueObjects;

public sealed class GradeLevelId : IntegerId<GradeLevelId>
{
    private GradeLevelId(int value) : base(value)
    {
    }

    public static GradeLevelId Create(int value)
    {
        return new GradeLevelId(value);
    }
}