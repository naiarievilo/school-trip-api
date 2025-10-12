using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.SchoolTripAggregate.ValueObjects;

public sealed class SchoolTripName : SimpleValueObject<SchoolTripName, string>, ISimpleValueObjectValidator<string>
{
    public static readonly int MaxLength = 256;

    internal SchoolTripName(string value) : base(Validate(value))
    {
    }

    public static string Validate(string? value)
    {
        if (string.IsNullOrEmpty(value)) throw new ValueObjectException("Trip name is required.");
        return value.Length <= MaxLength
            ? value
            : throw new ValueObjectException($"Trip name must not exceed {MaxLength} characters.");
    }
}