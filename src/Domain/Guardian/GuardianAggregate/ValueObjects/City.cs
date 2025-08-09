using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.Errors;
using SchoolTripApi.Domain.Common.Exceptions;
using SchoolTripApi.Domain.Common.Services;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

public class City : ValueObject
{
    public static readonly int MaxLength = 64;
    private static readonly Regex CityPattern = new(@"^[A-Za-zÀ-ÿ\s\-'\.]+$", RegexOptions.Compiled);

    private City(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ValueObjectValidationException("City is required.");
        value = value.Trim();
        if (value.Length > MaxLength) throw new ValueObjectValidationException("City name is too long.");
        if (!CityPattern.IsMatch(value))
            throw new ValueObjectValidationException(
                "City name must contain only letters, spaces, hyphens, periods, or apostrophes.");

        Value = value;
    }

    public string Value { get; }

    public static City From(string value)
    {
        return new City(value);
    }

    public static Result<City> TryFrom(string value)
    {
        try
        {
            var city = From(value);
            return Result.Success(city);
        }
        catch (ValueObjectValidationException ex)
        {
            return Result.Failure<City>(ValueObjectError.FailedToConvertToValueObject, ex.Message);
        }
    }

    public bool Equals(string? other)
    {
        return other is not null && PortugueseStringNormalizer.AreEquivalent(Value, other);
    }

    public bool Equals(City other)
    {
        return PortugueseStringNormalizer.AreEquivalent(Value, other.Value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}