using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Common.ValueObjects;

public sealed class State : SimpleValueObject<State, string>, ISimpleValueObjectValidator<string>
{
    public static readonly int MaxLength = 32;

    private State(string value) : base(Validate(value))
    {
    }

    public static string Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ValueObjectException("State is required.");
        return value.Length <= MaxLength
            ? value
            : throw new ValueObjectException($"State name must not exceed {MaxLength} characters.");
    }
}