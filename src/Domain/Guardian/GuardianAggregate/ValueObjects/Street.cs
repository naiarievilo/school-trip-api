using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.Errors;
using SchoolTripApi.Domain.Common.Exceptions;
using SchoolTripApi.Domain.Common.Services;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

public class Street : ValueObject
{
    public static readonly int MaxLength = 64;
    private static readonly Regex StreetPattern = new(@"^[a-zA-ZÀ-ÿ0-9\s\.\,\-\']+$", RegexOptions.Compiled);

    private Street(string value)
    {
        if (!string.IsNullOrWhiteSpace(value)) throw new ValueObjectException("Street is required.");
        if (value.Length > MaxLength) throw new ValueObjectException("Street name is too long.");
        if (!StreetPattern.IsMatch(value))
            throw new ValueObjectException(
                "Street should only contain letters, spaces, hyphens, commas, and apostrophes.");

        Value = Normalize(value);
    }

    public string Value { get; }

    public static Street From(string value)
    {
        return new Street(value);
    }

    public static Result<Street> TryFrom(string value)
    {
        try
        {
            var street = From(value);
            return Result.Success(street);
        }
        catch (ValueObjectException ex)
        {
            return Result.Failure<Street>(ValueObjectError.FailedToConvertToValueObject, ex.Message);
        }
    }

    private static string Normalize(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? value : value.Trim();
    }

    public bool Equals(string? other)
    {
        return other is not null && PortugueseStringNormalizer.AreEquivalent(Value, other);
    }

    public bool Equals(Street other)
    {
        return PortugueseStringNormalizer.AreEquivalent(Value, other.Value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}