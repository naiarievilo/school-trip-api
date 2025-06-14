using System.Text.RegularExpressions;
using Vogen;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

[ValueObject<string>]
public readonly partial struct PhoneNumber
{
    public static readonly int MaxLength = 21;

    private static readonly Regex PhoneNumberPattern = PhoneNumberRegex();

    private static string NormalizeInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;

        input = input.Trim();
        if (input.Length > MaxLength || !PhoneNumberPattern.IsMatch(input)) return input;

        var normalizedInput = SpecialCharactersRegex().Replace(input.Trim(), "");

        var parsedPhoneNumber = ParsePhoneNumber(normalizedInput);
        if (parsedPhoneNumber is null) return input;

        var areaCode = parsedPhoneNumber.Value.AreaCode;
        var lineNumber = parsedPhoneNumber.Value.LocalNumber;

        return $"+55{areaCode}{lineNumber}";
    }

    private static Validation Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return Validation.Invalid("Phone number is required.");
        if (input.Length > 20) return Validation.Invalid("Phone number is too long.");

        return !PhoneNumberPattern.IsMatch(input)
            ? Validation.Invalid("Phone number is not a valid phone number.")
            : Validation.Ok;
    }

    private static (string AreaCode, string LocalNumber, bool IsMobile)? ParsePhoneNumber(string normalizedNumber)
    {
        // Remove country code if present
        if (normalizedNumber.StartsWith("+55"))
            normalizedNumber = normalizedNumber[3..];
        else if (normalizedNumber.StartsWith("55"))
            normalizedNumber = normalizedNumber[2..];

        // Remove the leading zero from area code if present
        if (normalizedNumber.StartsWith("0"))
            normalizedNumber = normalizedNumber[1..];

        string areaCode;
        string localNumber;

        // Handle 11-digit mobile numbers (area code + 9 + 8 digits)
        if (normalizedNumber.Length == 11 && normalizedNumber[2] == '9')
        {
            areaCode = normalizedNumber[..2];
            localNumber = normalizedNumber[2..];
        }
        // Handle 10-digit numbers (area code + 8 digits for landline or old mobile format)
        else if (normalizedNumber.Length == 10)
        {
            areaCode = normalizedNumber[..2];
            localNumber = normalizedNumber[2..];
        }
        // Handle cases where area code might be 3 digits (rare cases)
        else if (normalizedNumber.Length == 12 && normalizedNumber[3] == '9')
        {
            areaCode = normalizedNumber[..3];
            localNumber = normalizedNumber[3..];
        }
        else if (normalizedNumber.Length == 11 && normalizedNumber[3] != '9')
        {
            areaCode = normalizedNumber[..3];
            localNumber = normalizedNumber[3..];
        }
        else
        {
            return null;
        }

        // Validate area code is numeric and within the valid range
        if (!int.TryParse(areaCode, out var areaCodeNum) || areaCodeNum < 11 || areaCodeNum > 99)
            return null;

        // Check if it's a mobile number (starts with 9 and has 9 digits total)
        var isMobile = localNumber.Length == 9 && localNumber[0] == '9';

        return (areaCode, localNumber, isMobile);
    }

    [GeneratedRegex(@"[\s\-\(\)\.]+", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex SpecialCharactersRegex();

    [GeneratedRegex(@"^(?:\+?55\s?)?(?:\(?(?:0?11|0?[12-9][0-9])\)?\s?)?(?:9\s?)?[2-9][0-9]{3}[\s\-]?[0-9]{4}$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex PhoneNumberRegex();
}