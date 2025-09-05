using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Common.ValueObjects;

public sealed class Street : SimpleValueObject<Street, string>, ISimpleValueObjectValidator<string>
{
    public static readonly int MaxLength = 64;

    private Street(string value) : base(Validate(value))
    {
    }

    public static string Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ValueObjectException("Street is required.");
        return value.Length <= MaxLength
            ? value
            : throw new ValueObjectException($"Street name must not exceed {MaxLength} characters.");
    }
}