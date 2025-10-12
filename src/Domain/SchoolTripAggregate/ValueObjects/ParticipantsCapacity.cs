using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.SchoolTripAggregate.ValueObjects;

public sealed class ParticipantsCapacity : ValueObject
{
    public ParticipantsCapacity(MinimumParticipants minimumRequired,
        MaximumParticipants maximumAllowed)
    {
        if (minimumRequired.Value > maximumAllowed.Value)
            throw new ValueObjectException(
                "Number of minimum participants cannot be greater than maximum participants.");

        MinimumRequired = minimumRequired;
        MaximumAllowed = maximumAllowed;
    }

    public MinimumParticipants MinimumRequired { get; }
    public MaximumParticipants MaximumAllowed { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return MinimumRequired;
        yield return MaximumAllowed;
    }
}