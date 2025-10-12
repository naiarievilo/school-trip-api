using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.SchoolTripAggregate.ValueObjects;

public sealed class SchoolTripSummary : SimpleValueObject<SchoolTripSummary, string>,
    ISimpleValueObjectValidator<string>
{
    public static readonly int MaxLength = 500;

    internal SchoolTripSummary(string value) : base(Validate(value))
    {
    }

    public static string Validate(string? value)
    {
        if (string.IsNullOrEmpty(value)) throw new ValueObjectException("Trip summary is required.");
        return value.Length <= MaxLength
            ? value
            : throw new ValueObjectException($"Trip summary must not exceed {MaxLength} characters.");
    }
}