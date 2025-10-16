using System.Text.RegularExpressions;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Common.ValueObjects;

public sealed partial class Rg : SimpleValueObject<Rg, string>, ISimpleValueObjectValidator<string>
{
    public static readonly int MaxLength = 13;
    private static readonly Regex RgPattern = RgRegex();

    internal Rg(string value) : base(Normalize(Validate(value)))
    {
    }

    public static string Validate(string? value)
    {
        if (string.IsNullOrEmpty(value)) throw new ValueObjectException("RG is required.");
        if (value.Length > MaxLength) throw new ValueObjectException($"RG cannot exceed {MaxLength} characters.");

        return RgPattern.IsMatch(value)
            ? value
            : throw new ValueObjectException("RG is invalid.");
    }

    [GeneratedRegex(@"^(\d{1,2}\.?\d{3}\.?\d{3}-?[\dXx]{0,2}|\d{7,10}[\dXx]?)$")]
    private static partial Regex RgRegex();

    private static string Normalize(string rg)
    {
        var cleanedRg = CleanRg(rg);
        return cleanedRg.Length switch
        {
            // Format with 2 check digits: XX.XXX.XXX-XX
            10 =>
                $"{cleanedRg.Substring(0, 2)}.{cleanedRg.Substring(2, 3)}.{cleanedRg.Substring(5, 3)}-{cleanedRg.Substring(8, 2)}",
            // Standard format: XX.XXX.XXX-X
            9 => $"{cleanedRg.Substring(0, 2)}.{cleanedRg.Substring(2, 3)}.{cleanedRg.Substring(5, 3)}-{cleanedRg[8]}",
            // Older format with 8 digits: X.XXX.XXX-X
            8 => $"{cleanedRg[0]}.{cleanedRg.Substring(1, 3)}.{cleanedRg.Substring(4, 3)}-{cleanedRg[7]}",
            _ => cleanedRg
        };
    }

    private static string CleanRg(string rg)
    {
        return SpecialCharacters().Replace(rg, string.Empty).ToUpperInvariant();
    }

    [GeneratedRegex(@"[.\-]")]
    private static partial Regex SpecialCharacters();
}