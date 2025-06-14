using System.Text.RegularExpressions;
using Vogen;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

[ValueObject<string>]
public readonly partial struct Cpf
{
    public static readonly int MaxLength = 14;
    private static readonly Regex CpfPattern = CpfRegex();

    private static Validation Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return Validation.Invalid("CPF is required.");
        if (input.Length > MaxLength) return Validation.Invalid("CPF is too long.");
        return !CpfPattern.IsMatch(input)
            ? Validation.Invalid("CPF provided is invalid.")
            : Validation.Ok;
    }

    [GeneratedRegex(@"(^\d{3}\.\d{3}\.\d{3}\-\d{2}$)", RegexOptions.Compiled)]
    private static partial Regex CpfRegex();

    private static string CpfDigits(string cpf)
    {
        return DigitsOnly().Replace(cpf.Trim(), string.Empty);
    }


    private static string FormattedCpf(string digitsOnly)
    {
        return $"{digitsOnly[..3]}.{digitsOnly[3..6]}.{digitsOnly[6..9]}-{digitsOnly[9..11]}";
    }

    private static string NormalizeInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;

        input = input.Trim();
        if (input.Length > MaxLength) return input;

        // Return a string that doesn't match the CPF pattern to fail validation.
        const string invalidCpf = "Invalid CPF.";
        var normalizedCpf = CpfDigits(input);
        return IsAllSameDigits(normalizedCpf) || !IsValidCpf(normalizedCpf)
            ? invalidCpf
            : FormattedCpf(normalizedCpf);
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

    [GeneratedRegex(@"[^\d]", RegexOptions.Compiled)]
    private static partial Regex DigitsOnly();
}