using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Enums;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Common.ValueObjects;

public sealed partial class PhoneNumber : SimpleValueObject<PhoneNumber, string>, ISimpleValueObjectValidator<string>
{
    public static readonly int MaxLength = 21;

    private static readonly Regex PhoneNumberPattern = PhoneNumberRegex();
    private static readonly Regex SpecialCharactersPattern = SpecialCharactersRegex();

    private PhoneNumber(string value) : base(Validate(value))
    {
    }

    public static string Validate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValueObjectException("Phone number is required.");
        if (value.Length > MaxLength) throw new ValueObjectException("Phone number is too long.");
        return PhoneNumberPattern.IsMatch(value)
            ? Normalize(value)
            : throw new ValueObjectException("Phone number is not a valid phone number.");
    }

    private static string Normalize(string phoneNumber)
    {
        var normalizedPhoneNumber = NormalizePhoneNumber(phoneNumber);
        var (areaCode, lineNumber) = ParseNormalizedPhoneNumber(normalizedPhoneNumber);
        return $"+55{areaCode}{lineNumber}";
    }

    private static string NormalizePhoneNumber(string phoneNumber)
    {
        return SpecialCharactersPattern.Replace(phoneNumber.Trim(), "");
    }

    public bool IsMobile()
    {
        var normalizedLineNumber = GetNormalizedLineNumber(Value);
        return IsValidMobileNumber(normalizedLineNumber);
    }

    public bool IsLandline()
    {
        var normalizedLineNumber = GetNormalizedLineNumber(Value);
        return IsValidLandlineNumber(normalizedLineNumber);
    }

    private static string GetNormalizedLineNumber(string normalizedPhoneNumber)
    {
        return normalizedPhoneNumber[5 ..];
    }

    private static (string AreaCode, string LineNumber) ParseNormalizedPhoneNumber(
        string normalizedPhoneNumber)
    {
        // Remove country code if present
        if (normalizedPhoneNumber.StartsWith("+55"))
            normalizedPhoneNumber = normalizedPhoneNumber[3..];
        else if (normalizedPhoneNumber.StartsWith("55"))
            normalizedPhoneNumber = normalizedPhoneNumber[2..];

        // Remove the leading zero from area code if present
        if (normalizedPhoneNumber.StartsWith("0"))
            normalizedPhoneNumber = normalizedPhoneNumber[1..];

        string areaCode;
        string lineNumber;
        PhoneType phoneType;

        // Handle 11-digit mobile numbers (area code + 9 + 8 digits)
        if (normalizedPhoneNumber.Length == 11 && normalizedPhoneNumber[2] == '9')
        {
            areaCode = normalizedPhoneNumber[..2];
            lineNumber = normalizedPhoneNumber[2..];
            phoneType = PhoneType.Mobile;
        }

        // Handle 10-digit numbers (area code + 8 digits for landline or old mobile format)
        else if (normalizedPhoneNumber.Length == 10)
        {
            areaCode = normalizedPhoneNumber[..2];
            lineNumber = normalizedPhoneNumber[2..];

            // Determine type based on the first digit of local numbers
            // Mobile: starts with 6, 7, 8, or 9
            // Landline: starts with 2, 3, 4, or 5
            phoneType = lineNumber[0] >= 6 ? PhoneType.Mobile : PhoneType.Landline;
        }

        // Handle cases where area code might be 3 digits (rare cases)
        else if (normalizedPhoneNumber.Length == 12 && normalizedPhoneNumber[3] == '9')
        {
            areaCode = normalizedPhoneNumber[..3];
            lineNumber = normalizedPhoneNumber[3..];
            phoneType = PhoneType.Mobile;
        }

        else if (normalizedPhoneNumber.Length == 11 && normalizedPhoneNumber[3] != '9')
        {
            areaCode = normalizedPhoneNumber[..3];
            lineNumber = normalizedPhoneNumber[3..];
            phoneType = PhoneType.Landline;
        }
        else
        {
            throw new ValueObjectException($"Normalized phone number '{normalizedPhoneNumber}' couldn't be parsed.");
        }

        // Validate area code is numeric and within the valid range
        if (!int.TryParse(areaCode, out var parsedAreaCode) || parsedAreaCode < 11 || parsedAreaCode > 99)
            throw new ValueObjectException("Normalized phone number are code is not within valid range.");

        return IsValidPhoneType(lineNumber, phoneType)
            ? (areaCode, localNumber: lineNumber)
            : throw new ValueObjectException("Normalized phone number does not have a valid phone type.");
    }

    private static bool IsValidPhoneType(string normalizedLocalNumber, PhoneType phoneType)
    {
        return phoneType switch
        {
            PhoneType.Mobile => IsValidMobileNumber(normalizedLocalNumber),
            PhoneType.Landline => IsValidLandlineNumber(normalizedLocalNumber),
            _ => false
        };
    }

    private static bool IsValidMobileNumber(string normalizedLocalNumber)
    {
        // Mobile numbers: 9XXXX-XXXX (9 digits, starts with 9)
        return normalizedLocalNumber.Length == 9 &&
               normalizedLocalNumber[0] == '9' &&
               normalizedLocalNumber.All(char.IsDigit);
    }

    private static bool IsValidLandlineNumber(string normalizedLocalNumber)
    {
        // Landline numbers: XXXX-XXXX (8 digits, starts with 2-5)
        return normalizedLocalNumber.Length == 8 &&
               normalizedLocalNumber[0] >= '2' && normalizedLocalNumber[0] <= '5' &&
               normalizedLocalNumber.All(char.IsDigit);
    }

    // Handles both mobile and landline Brazilian phone number patterns
    [GeneratedRegex(
        @"^(?:\+?55\s?)?(?:\(?(?:0?11|0?[12-9][0-9])\)?\s?)?(?:(?:9\s?[2-9][0-9]{3})|(?:[2-5][0-9]{3}))[\s\-]?[0-9]{4}$",
        RegexOptions.IgnoreCase
    )]
    private static partial Regex PhoneNumberRegex();

    [GeneratedRegex(@"[\s\-\(\)\.]+", RegexOptions.IgnoreCase)]
    private static partial Regex SpecialCharactersRegex();
}