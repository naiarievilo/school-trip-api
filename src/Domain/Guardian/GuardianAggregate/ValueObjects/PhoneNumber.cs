using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.Errors;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

public class PhoneNumber : ValueObject
{
    public static readonly int MaxLength = 21;

    private static readonly Regex PhoneNumberPattern = new(
        @"^(?:\+?55\s?)?(?:\(?(?:0?11|0?[12-9][0-9])\)?\s?)?(?:9\s?)?[2-9][0-9]{3}[\s\-]?[0-9]{4}$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex SpecialCharactersRegex =
        new(@"[\s\-\(\)\.]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValueObjectValidationException("Phone number is required.");
        if (value.Length > 20) throw new ValueObjectValidationException("Phone number is too long.");

        if (!PhoneNumberPattern.IsMatch(value))
            throw new ValueObjectValidationException("Phone number is not a valid phone number.");

        Value = Normalize(value);
    }

    public string Value { get; }

    private static string Normalize(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;

        value = value.Trim();
        if (value.Length > MaxLength || !PhoneNumberPattern.IsMatch(value)) return value;

        var normalizedInput = SpecialCharactersRegex.Replace(value.Trim(), "");

        var parsedPhoneNumber = ParseNormalizedPhoneNumber(normalizedInput);
        if (parsedPhoneNumber is null) return value;

        var areaCode = parsedPhoneNumber.Value.AreaCode;
        var lineNumber = parsedPhoneNumber.Value.LocalNumber;

        return $"+55{areaCode}{lineNumber}";
    }

    public static PhoneNumber From(string value)
    {
        return new PhoneNumber(value);
    }

    public static Result<PhoneNumber> TryFrom(string value)
    {
        try
        {
            var phoneNumber = From(value);
            return Result.Success(phoneNumber);
        }
        catch (ValueObjectValidationException ex)
        {
            return Result.Failure<PhoneNumber>(ValueObjectError.FailedToConvertToValueObject, ex.Message);
        }
    }

    private static (string AreaCode, string LocalNumber, bool IsMobile)? ParseNormalizedPhoneNumber(
        string normalizedNumber)
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

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}