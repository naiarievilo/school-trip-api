using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Services;
using Vogen;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

[ValueObject<string>]
public readonly partial struct Neighborhood
{
    public static readonly int MaxLength = 64;
    private static readonly Regex NeighborhoodPattern = new(@"^[A-Za-zÀ-ÿ\s\-'\.]+$", RegexOptions.Compiled);

    private static Validation Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return Validation.Invalid("Neighborhood is required.");
        if (input.Length > MaxLength) return Validation.Invalid("Neighborhood name is too long.");
        return !NeighborhoodPattern.IsMatch(input)
            ? Validation.Invalid("Neighborhood name must contain only letters, hyphens, periods, and apostrophes.")
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

    public bool Equals(Neighborhood other)
    {
        return PortugueseStringNormalizer.AreEquivalent(Value, other.Value);
    }
}