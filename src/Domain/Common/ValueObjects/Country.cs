using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Common.ValueObjects;

public sealed class Country : SimpleValueObject<Country, string>, ISimpleValueObjectValidator<string>
{
    public static readonly int MaxLength = 64;

    private Country(string value) : base(Validate(value))
    {
    }

    public static string Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ValueObjectException("Country is required.");
        return value.Length <= MaxLength
            ? value
            : throw new ValueObjectException($"Country name must not exceed {MaxLength} characters.");
    }
}