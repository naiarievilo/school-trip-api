using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.TripAggregate.ValueObjects;

public sealed class ParticipantsCapacity : ValueObject
{
    public ParticipantsCapacity(MinimumTripParticipants minimumRequired,
        MaximumTripParticipants maximumAllowed)
    {
        if (minimumRequired.Value > maximumAllowed.Value)
            throw new ValueObjectException(
                "Number of minimum participants cannot be greater than maximum participants.");

        MinimumRequired = minimumRequired;
        MaximumAllowed = maximumAllowed;
    }

    public MinimumTripParticipants MinimumRequired { get; }
    public MaximumTripParticipants MaximumAllowed { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return MinimumRequired;
        yield return MaximumAllowed;
    }
}