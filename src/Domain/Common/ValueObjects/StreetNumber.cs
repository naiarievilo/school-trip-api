using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Common.ValueObjects;

public sealed class StreetNumber : SimpleValueObject<StreetNumber, string>, ISimpleValueObjectValidator<string>
{
    public static readonly int MaxLength = 16;
    private static readonly Regex StreetNumberPattern = new(@"^[0-9A-Za-z\-/]+$", RegexOptions.Compiled);

    private StreetNumber(string value) : base(Validate(value))
    {
    }

    public static string Validate(string value)
    {
        if (string.IsNullOrEmpty(value)) throw new ValueObjectException("Street number is required.");
        if (value.Length > MaxLength) throw new ValueObjectException("Street number is too long.");
        return StreetNumberPattern.IsMatch(value)
            ? value
            : throw new ValueObjectException(
                "Must contain only digits (0-9), hyphen (-), foward slash (/), or 'S' and 'N' letters.");
    }
}