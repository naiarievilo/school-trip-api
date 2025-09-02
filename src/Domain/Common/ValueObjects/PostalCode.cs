using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.Errors;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Common.ValueObjects;

public class PostalCode : ValueObject
{
    public static readonly int MaxLength = 9;
    private static readonly Regex CepPattern = new(@"^[0-9]{5}-?[0-9]{3}$", RegexOptions.Compiled);

    private PostalCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ValueObjectException("Postal code is required.");
        if (value.Length > MaxLength) throw new ValueObjectException("Postal code is too long.");
        if (!CepPattern.IsMatch(value))
            throw new ValueObjectException("CEP must only contain digits and an optional hyphen.");

        Value = Normalize(value);
    }

    public string Value { get; }

    public static PostalCode From(string value)
    {
        return new PostalCode(value);
    }

    public static Result<PostalCode> TryFrom(string value)
    {
        try
        {
            var postalCode = From(value);
            return Result.Success(postalCode);
        }
        catch (ValueObjectException ex)
        {
            return Result.Failure<PostalCode>(ValueObjectError.FailedToConvertToValueObject, ex.Message);
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