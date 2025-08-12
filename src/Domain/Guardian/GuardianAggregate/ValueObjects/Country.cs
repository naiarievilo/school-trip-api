using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.Errors;
using SchoolTripApi.Domain.Common.Exceptions;
using SchoolTripApi.Domain.Common.Services;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

public class Country : ValueObject
{
    public static readonly int MaxLength = 64;
    private static readonly Regex CountryPattern = new(@"^[A-Za-zÀ-ÿ\s\-]+$", RegexOptions.Compiled);

    private Country(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ValueObjectException("Country is required.");
        if (value.Length > MaxLength) throw new ValueObjectException("Country name is too long.");
        if (!CountryPattern.IsMatch(value))
            throw new ValueObjectException("Country name must contain only letters, spaces, and hyphen.");

        Value = value;
    }

    public string Value { get; }

    public static Country From(string value)
    {
        return new Country(value);
    }

    public static Result<Country> TryFrom(string value)
    {
        try
        {
            var country = From(value);
            return Result.Success(country);
        }
        catch (ValueObjectException ex)
        {
            return Result.Failure<Country>(ValueObjectError.FailedToConvertToValueObject, ex.Message);
        }
    }

    public bool Equals(string? other)
    {
        return other is not null && PortugueseStringNormalizer.AreEquivalent(Value, other);
    }

    public bool Equals(Country other)
    {
        return PortugueseStringNormalizer.AreEquivalent(Value, other.Value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}