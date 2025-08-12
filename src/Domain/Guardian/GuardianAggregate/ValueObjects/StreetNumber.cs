using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.Errors;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

public class StreetNumber : ValueObject
{
    public static readonly int MaxLength = 16;
    private static readonly Regex StreetNumberPattern = new(@"^[0-9A-Za-z\-/]+$", RegexOptions.Compiled);

    private StreetNumber(string value)
    {
        if (!string.IsNullOrEmpty(value)) throw new ValueObjectException("Street number is required.");
        if (value.Length > MaxLength) throw new ValueObjectException("Street number is too long.");
        if (!StreetNumberPattern.IsMatch(value))
            throw new ValueObjectException(
                "Must contain only digits (0-9), hyphen (-), foward slash (/), or 'S' and 'N' letters.");

        Value = Normalize(value);
    }

    public string Value { get; }

    public static StreetNumber From(string value)
    {
        return new StreetNumber(value);
    }

    public static Result<StreetNumber> TryFrom(string value)
    {
        try
        {
            var streetNumber = From(value);
            return Result.Success(streetNumber);
        }
        catch (ValueObjectException ex)
        {
            return Result.Failure<StreetNumber>(ValueObjectError.FailedToConvertToValueObject, ex.Message);
        }
    }

    private static string Normalize(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? value : value.Trim();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}