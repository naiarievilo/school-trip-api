using System.Text.RegularExpressions;
using Vogen;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

[ValueObject<string>]
public readonly partial struct FullName
{
    public static readonly int MaxLength = 128;
    private static readonly Regex FullNamePattern = FullNameRegex();

    private static Validation Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return Validation.Invalid("Full name is required.");
        if (input.Length > MaxLength) return Validation.Invalid("Full name is too long.");
        return !FullNamePattern.IsMatch(input)
            ? Validation.Invalid("Full name must contain letters, hyphens, and apostrophes only.")
            : Validation.Ok;
    }

    private static string NormalizeInput(string input)
    {
        return string.IsNullOrWhiteSpace(input) ? input : input.Trim();
    }

    [GeneratedRegex(@"^[A-Za-zÀ-ÿ\s\-']+$", RegexOptions.Compiled)]
    private static partial Regex FullNameRegex();
}