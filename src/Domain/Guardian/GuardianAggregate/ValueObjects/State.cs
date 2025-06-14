using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Services;
using Vogen;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

[ValueObject<string>]
public readonly partial struct State
{
    public static readonly int MaxLength = 32;
    private static readonly Regex StatePattern = new(@"^([A-Z]{2}|[A-Za-zÀ-ÿ\s\-'\.]+)$", RegexOptions.Compiled);

    private static string NormalizeInput(string input)
    {
        return string.IsNullOrWhiteSpace(input) ? input : input.Trim();
    }

    private static Validation Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return Validation.Invalid("State is required.");
        if (input.Length > MaxLength) return Validation.Invalid("State name is too long.");
        return !StatePattern.IsMatch(input)
            ? Validation.Invalid("State name must contain only letters, spaces, hyphens, periods, and apostrophes.")
            : Validation.Ok;
    }

    public bool Equals(string? other)
    {
        return other is not null && PortugueseStringNormalizer.AreEquivalent(Value, other);
    }

    public bool Equals(State other)
    {
        return PortugueseStringNormalizer.AreEquivalent(Value, other.Value);
    }
}