using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Services;
using Vogen;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

[ValueObject<string>]
public readonly partial struct ContactName
{
    public static readonly int MaxLength = 32;
    private static readonly Regex ContactNamePattern = new(@"^[A-Za-zÀ-ÿ\s\-']+$", RegexOptions.Compiled);

    private static string NormalizeInput(string input)
    {
        return string.IsNullOrWhiteSpace(input) ? input : input.Trim();
    }

    private static Validation Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return Validation.Invalid("Contact name is required.");
        if (input.Length > MaxLength) return Validation.Invalid("Contact name is too long.");
        return !ContactNamePattern.IsMatch(input)
            ? Validation.Invalid("Contact name must contain only letters, hyphens, and apostrophes.")
            : Validation.Ok;
    }

    public bool Equals(string? other)
    {
        return other is not null && PortugueseStringNormalizer.AreEquivalent(Value, other);
    }

    public bool Equals(ContactName other)
    {
        return PortugueseStringNormalizer.AreEquivalent(Value, other.Value);
    }
}