using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Common.ValueObjects;

public sealed partial class Cep : SimpleValueObject<Cep, string>, ISimpleValueObjectValidator<string>
{
    public static readonly int MaxLength = 9;
    private static readonly Regex CepPattern = CepRegex();

    private Cep(string value) : base(Validate(value))
    {
    }

    public static string Validate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ValueObjectException("CEP is required.");
        if (value.Length > MaxLength)
            throw new ValueObjectException($"CEP must not exceed {MaxLength} characters.");
        return CepPattern.IsMatch(value)
            ? value
            : throw new ValueObjectException("CEP must only contain digits and an optional hyphen.");
    }

    [GeneratedRegex(@"^[0-9]{5}-?[0-9]{3}$")]
    private static partial Regex CepRegex();
}