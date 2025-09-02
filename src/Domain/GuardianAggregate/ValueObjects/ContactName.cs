using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.Errors;
using SchoolTripApi.Domain.Common.Exceptions;
using SchoolTripApi.Domain.Common.Services;

namespace SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

public class ContactName : ValueObject
{
    public static readonly int MaxLength = 32;
    private static readonly Regex ContactNamePattern = new(@"^[A-Za-zÀ-ÿ\s\-']+$", RegexOptions.Compiled);

    private ContactName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValueObjectException("Contact name is required.");
        if (value.Length > MaxLength) throw new ValueObjectException("Contact name is too long.");
        if (!ContactNamePattern.IsMatch(value))
            throw new ValueObjectException(
                "Contact name must contain only letters, hyphens, and apostrophes.");

        Value = value;
    }

    public string Value { get; }

    public static ContactName From(string value)
    {
        return new ContactName(value);
    }

    public static Result<ContactName> TryFrom(string value)
    {
        try
        {
            var contactName = From(value);
            return Result.Success(contactName);
        }
        catch (ValueObjectException ex)
        {
            return Result.Failure<ContactName>(ValueObjectError.FailedToConvertToValueObject, ex.Message);
        }
    }

    public bool Equals(string? other)
    {
        return other is not null && PortugueseStringNormalizer.AreEquivalent(Value, other);
    }

    public bool Equals(ContactName other)
    {
        return PortugueseStringNormalizer.AreEquivalent(Value, other.Value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}