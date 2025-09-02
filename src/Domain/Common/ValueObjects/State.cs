using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.Errors;
using SchoolTripApi.Domain.Common.Exceptions;
using SchoolTripApi.Domain.Common.Services;

namespace SchoolTripApi.Domain.Common.ValueObjects;

public class State : ValueObject
{
    public static readonly int MaxLength = 32;
    private static readonly Regex StatePattern = new(@"^([A-Z]{2}|[A-Za-zÀ-ÿ\s\-'\.]+)$", RegexOptions.Compiled);

    private State(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ValueObjectException("State is required.");
        if (value.Length > MaxLength) throw new ValueObjectException("State name is too long.");
        if (!StatePattern.IsMatch(value))
            throw new ValueObjectException(
                "State name must contain only letters, spaces, hyphens, periods, and apostrophes.");

        Value = Normalize(value);
    }

    public string Value { get; }

    private static string Normalize(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? value : value.Trim();
    }

    public static State From(string value)
    {
        return new State(value);
    }

    public static Result<State> TryFrom(string value)
    {
        try
        {
            var state = From(value);
            return Result.Success(state);
        }
        catch (ValueObjectException ex)
        {
            return Result.Failure<State>(ValueObjectError.FailedToConvertToValueObject, ex.Message);
        }
    }

    public bool Equals(string? other)
    {
        return other is not null && PortugueseStringNormalizer.AreEquivalent(Value, other);
    }

    public bool Equals(State other)
    {
        return PortugueseStringNormalizer.AreEquivalent(Value, other.Value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}