using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Services;
using Vogen;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

[ValueObject<string>]
public readonly partial struct Country
{
    public static readonly int MaxLength = 64;
    private static readonly Regex CountryPattern = new(@"^[A-Za-zÀ-ÿ\s\-]+$", RegexOptions.Compiled);

    private static string NormalizeInput(string input)
    {
        return string.IsNullOrWhiteSpace(input) ? input : input.Trim();
    }

    private static Validation Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return Validation.Invalid("Country is required.");
        if (input.Length > MaxLength) return Validation.Invalid("Country name is too long.");
        return !CountryPattern.IsMatch(input)
            ? Validation.Invalid("Country name must contain only letters, spaces, and hyphen.")
            : Validation.Ok;
    }

    public bool Equals(string? other)
    {
        return other is not null && PortugueseStringNormalizer.AreEquivalent(Value, other);
    }

    public bool Equals(Country other)
    {
        return PortugueseStringNormalizer.AreEquivalent(Value, other.Value);
    }
}