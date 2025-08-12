using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.Errors;
using SchoolTripApi.Domain.Common.Exceptions;
using SchoolTripApi.Domain.Common.Services;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

public class Neighborhood : ValueObject
{
    public static readonly int MaxLength = 64;
    private static readonly Regex NeighborhoodPattern = new(@"^[A-Za-zÀ-ÿ\s\-'\.]+$", RegexOptions.Compiled);

    private Neighborhood(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValueObjectException("Neighborhood is required.");
        if (value.Length > MaxLength) throw new ValueObjectException("Neighborhood name is too long.");
        if (!NeighborhoodPattern.IsMatch(value))
            throw new ValueObjectException(
                "Neighborhood name must contain only letters, hyphens, periods, and apostrophes.");

        Value = Normalize(value);
    }

    public string Value { get; }

    public static Neighborhood From(string value)
    {
        return new Neighborhood(value);
    }

    public static Result<Neighborhood> TryFrom(string value)
    {
        try
        {
            var neighborhood = From(value);
            return Result.Success(neighborhood);
        }
        catch (ValueObjectException ex)
        {
            return Result.Failure<Neighborhood>(ValueObjectError.FailedToConvertToValueObject, ex.Message);
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

    public bool Equals(Neighborhood other)
    {
        return PortugueseStringNormalizer.AreEquivalent(Value, other.Value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}