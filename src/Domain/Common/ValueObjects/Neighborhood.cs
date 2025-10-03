using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Common.ValueObjects;

public sealed class Neighborhood : SimpleValueObject<Neighborhood, string>, ISimpleValueObjectValidator<string>
{
    public static readonly int MaxLength = 64;

    private Neighborhood(string value) : base(Validate(value))
    {
    }

    public static string Validate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ValueObjectException("Neighborhood is required.");
        return value.Length <= MaxLength
            ? value
            : throw new ValueObjectException($"Neighborhood name must not exceed {MaxLength} characters.");
    }
}