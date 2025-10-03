using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.RatingAggregate.ValueObjects;

public sealed class Comment : SimpleValueObject<Comment, string>, ISimpleValueObjectValidator<string>
{
    public static readonly int MinLength = 1;
    public static readonly int MaxLength = 1024;

    internal Comment(string value) : base(Validate(value))
    {
    }

    public static string Validate(string? value)
    {
        if (string.IsNullOrEmpty(value)) throw new ValueObjectException("Comment is required.");
        return value.Length < MinLength || value.Length > MaxLength
            ? value
            : throw new ValueObjectException($"Comment must have between {MinLength} and {MaxLength} characters.");
    }
}