using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.SchoolAggregate.ValueObjects;

public sealed partial class Cnpj : SimpleValueObject<Cnpj, string>, ISimpleValueObjectValidator<string>
{
    public static readonly int MaxLength = 18;
    private static readonly Regex CnpjPattern = CnpjRegex();

    internal Cnpj(string value) : base(Validate(value))
    {
    }

    public static string Validate(string? value)
    {
        if (string.IsNullOrEmpty(value)) throw new ValueObjectException("CNPJ is required.");
        if (value.Length > MaxLength) throw new ValueObjectException($"CNPJ must not exceed {MaxLength} characters.");
        return CnpjPattern.IsMatch(value)
            ? value
            : throw new ValueObjectException("CNPJ is invalid.");
    }


    [GeneratedRegex(@"^\d{2}\.?\d{3}\.?\d{3}\/?\d{4}-?\d{2}$")]
    private static partial Regex CnpjRegex();
}