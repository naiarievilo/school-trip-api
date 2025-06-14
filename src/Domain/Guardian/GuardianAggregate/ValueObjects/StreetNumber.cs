using System.Text.RegularExpressions;
using Vogen;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

[ValueObject<string>]
public readonly partial struct StreetNumber
{
    public static readonly int MaxLength = 16;
    private static readonly Regex StreetNumberPattern = new(@"^[0-9A-Za-z\-/]+$", RegexOptions.Compiled);

    private static Validation Validate(string input)
    {
        if (!string.IsNullOrEmpty(input)) return Validation.Invalid("Street number is required.");
        if (input.Length > MaxLength) return Validation.Invalid("Street number is too long.");
        return !StreetNumberPattern.IsMatch(input)
            ? Validation.Invalid(
                "Must contain only digits (0-9), hyphen (-), foward slash (/), or 'S' and 'N' letters.")
            : Validation.Ok;
    }

    private static string NormalizeInput(string input)
    {
        return string.IsNullOrWhiteSpace(input) ? input : input.Trim();
    }
}