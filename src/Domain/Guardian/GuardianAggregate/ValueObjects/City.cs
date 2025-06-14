using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Services;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

public class City : ValueObject
{
    public static readonly int MaxLength = 64;
    private static readonly Regex CityPattern = new(@"^[A-Za-zÀ-ÿ\s\-'\.]+$", RegexOptions.Compiled);
    
    public string Value { get; private set; }

    private City(string city)
    {
        if (string.IsNullOrWhiteSpace(city)) return Validation.Invalid("City is required.");
        if (input.Length > MaxLength) return Validation.Invalid("City name is too long.");
        return !CityPattern.IsMatch(input)
            ? Validation.Invalid("City name must contain only letters, spaces, hyphens, periods, or apostrophes.")
            : Validation.Ok;
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
        throw new NotImplementedException();
    }
}