using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.Errors;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

public class FullName : ValueObject
{
    public static readonly int MaxLength = 128;
    private static readonly Regex FullNamePattern = new(@"^[A-Za-zÀ-ÿ\s\-']+$", RegexOptions.Compiled);

    private FullName(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ValueObjectException("Full name is required.");
        if (value.Length > MaxLength) throw new ValueObjectException("Full name is too long.");
        if (!FullNamePattern.IsMatch(value))
            throw new ValueObjectException("Full name must contain letters, hyphens, and apostrophes only.");

        Value = Normalize(value);
    }

    public string Value { get; }

    public static FullName From(string value)
    {
        return new FullName(value);
    }

    public static Result<FullName> TryFrom(string value)
    {
        try
        {
            var fullName = From(value);
            return Result.Success(fullName);
        }
        catch (ValueObjectException ex)
        {
            return Result.Failure<FullName>(ValueObjectError.FailedToConvertToValueObject, ex.Message);
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