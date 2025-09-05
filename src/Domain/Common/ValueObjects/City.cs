using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Common.ValueObjects;

public sealed class City : SimpleValueObject<City, string>, ISimpleValueObjectValidator<string>
{
    public static readonly int MaxLength = 64;

    private City(string value) : base(Validate(value))
    {
    }

    public static string Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ValueObjectException("City is required.");
        return value.Length <= MaxLength
            ? value
            : throw new ValueObjectException($"City name must not exceed {MaxLength} characters.");
    }
}