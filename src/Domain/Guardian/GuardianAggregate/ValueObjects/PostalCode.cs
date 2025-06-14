using System.Text.RegularExpressions;
using Vogen;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

[ValueObject<string>]
public readonly partial struct PostalCode
{
    public static readonly int MaxLength = 9;
    private static readonly Regex CepPattern = new(@"^[0-9]{5}-?[0-9]{3}$", RegexOptions.Compiled);

    private static string NormalizeInput(string input)
    {
        return string.IsNullOrWhiteSpace(input) ? input : input.Trim();
    }

    private static Validation Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return Validation.Invalid("Postal code is required.");
        if (input.Length > MaxLength) return Validation.Invalid("Postal code is too long.");
        return !CepPattern.IsMatch(input)
            ? Validation.Invalid("CEP must only contain digits and an optional hyphen.")
            : Validation.Ok;
    }
}