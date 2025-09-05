using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

public sealed class Cpf : SimpleValueObject<Cpf, string>, ISimpleValueObjectValidator<string>
{
    public static readonly int MaxLength = 14;
    private static readonly Regex CpfPattern = new(@"(^\d{3}\.\d{3}\.\d{3}\-\d{2}$)", RegexOptions.Compiled);
    private static readonly Regex DigitsOnly = new(@"[^\d]", RegexOptions.Compiled);

    private Cpf(string value) : base(Normalize(Validate(value)))
    {
    }

    public static string Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ValueObjectException("CPF is required.");
        if (value.Length > MaxLength) throw new ValueObjectException("CPF is too long.");
        return CpfPattern.IsMatch(value) ? value : throw new ValueObjectException("CPF provided is invalid.");
    }

    private static string CpfDigits(string cpf)
    {
        return DigitsOnly.Replace(cpf.Trim(), string.Empty);
    }

    private static string FormattedCpf(string digitsOnly)
    {
        return $"{digitsOnly[..3]}.{digitsOnly[3..6]}.{digitsOnly[6..9]}-{digitsOnly[9..11]}";
    }

    private static string Normalize(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;

        value = value.Trim();
        if (value.Length > MaxLength) return value;

        var normalizedCpf = CpfDigits(value);
        if (IsAllSameDigits(normalizedCpf) || !IsValidCpf(normalizedCpf))
            throw new ValueObjectException("CPF provided is invalid.");

        return FormattedCpf(normalizedCpf);
    }

    private static bool IsAllSameDigits(string cpf)
    {
        return cpf.All(c => c == cpf[0]);
    }

    private static bool IsValidCpf(string cpf)
    {
        // Check the first verifier digit
        var sum = 0;
        for (var i = 0; i < 9; i++) sum += int.Parse(cpf[i].ToString()) * (10 - i);

        var firstCheckDigit = sum % 11;
        firstCheckDigit = firstCheckDigit < 2 ? 0 : 11 - firstCheckDigit;

        if (int.Parse(cpf[9].ToString()) != firstCheckDigit) return false;

        // Calculate the second verifier digit
        sum = 0;
        for (var i = 0; i < 10; i++) sum += int.Parse(cpf[i].ToString()) * (11 - i);

        var secondCheckDigit = sum % 11;
        secondCheckDigit = secondCheckDigit < 2 ? 0 : 11 - secondCheckDigit;

        return int.Parse(cpf[10].ToString()) == secondCheckDigit;
    }
}