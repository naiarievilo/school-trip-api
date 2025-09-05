using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

public sealed class ContactName : SimpleValueObject<ContactName, string>, ISimpleValueObjectValidator<string>
{
    public static readonly int MaxLength = 32;

    private ContactName(string value) : base(Validate(value))
    {
    }

    public static string Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValueObjectException("Contact name is required.");
        return value.Length <= MaxLength
            ? value
            : throw new ValueObjectException("Contact name is too long.");
    }
}