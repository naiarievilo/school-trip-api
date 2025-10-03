using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Common.ValueObjects;

public sealed class FullName : SimpleValueObject<FullName, string>, ISimpleValueObjectValidator<string>
{
    public static readonly int MaxLength = 256;

    private FullName(string value) : base(Validate(value))
    {
    }

    public static string Validate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ValueObjectException("Full name is required.");
        return value.Length <= MaxLength ? value : throw new ValueObjectException("Full name is too long.");
    }
}