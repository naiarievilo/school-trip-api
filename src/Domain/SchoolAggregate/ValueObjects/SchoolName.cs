using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.SchoolAggregate.ValueObjects;

public class SchoolName : SimpleValueObject<SchoolName, string>, ISimpleValueObjectValidator<string>
{
    public static readonly int MaxLength = 256;

    internal SchoolName(string value) : base(Validate(value))
    {
    }

    public static string Validate(string? value)
    {
        if (string.IsNullOrEmpty(value)) throw new ValueObjectException("School name is required.");
        return value.Length <= MaxLength
            ? value
            : throw new ValueObjectException($"School name must not exceed {MaxLength} characters.");
    }
}