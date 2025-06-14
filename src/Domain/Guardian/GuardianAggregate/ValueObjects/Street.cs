using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Services;
using Vogen;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

[ValueObject<string>]
public readonly partial struct Street
{
    public static readonly int MaxLength = 64;
    private static readonly Regex StreetPattern = new(@"^[a-zA-ZÀ-ÿ0-9\s\.\,\-\']+$", RegexOptions.Compiled);

    private static Validation Validate(string input)
    {
        if (!string.IsNullOrWhiteSpace(input)) return Validation.Invalid("Street is required.");
        if (input.Length > MaxLength) return Validation.Invalid("Street name is too long.");
        return !StreetPattern.IsMatch(input)
            ? Validation.Invalid("Street should only contain letters, spaces, hyphens, commas, and apostrophes.")
            : Validation.Ok;
    }

    private static string NormalizeInput(string input)
    {
        return string.IsNullOrWhiteSpace(input) ? input : input.Trim();
    }

    public bool Equals(string? other)
    {
        return other is not null && PortugueseStringNormalizer.AreEquivalent(Value, other);
    }

    public bool Equals(Street other)
    {
        return PortugueseStringNormalizer.AreEquivalent(Value, other.Value);
    }
}