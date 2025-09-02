using System.Globalization;
using System.Text;

namespace SchoolTripApi.Domain.Common.Services;

public static class PortugueseStringNormalizer
{
    // Method to remove accents from a Portuguese string
    private static string RemoveAccents(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        // Normalize to FormD (decomposed form) - separates base characters from diacritics
        var normalizedString = text.Normalize(NormalizationForm.FormD);

        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            // Keep only characters that are NOT 'NonSpacingMark' (i.e., diacritics/accents)
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark) stringBuilder.Append(c);
        }

        // Normalize back to FormC (composed form)
        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    // Method to normalize for comparison (i.e., case-insensitive, accent-insensitive)
    private static string NormalizeForComparison(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        return RemoveAccents(text.ToLowerInvariant().Trim());
    }

    // Method to normalize by preserving casing but removing accents
    public static string NormalizePreservingCase(string text)
    {
        return string.IsNullOrEmpty(text) ? text : RemoveAccents(text.Trim());
    }

    // Method to check if two Portuguese strings are equivalent (ignoring accents and case)
    public static bool AreEquivalent(string text1, string text2)
    {
        return string.Equals(
            NormalizeForComparison(text1),
            NormalizeForComparison(text2),
            StringComparison.Ordinal
        );
    }

    // Method using StringComparer for Portuguese culture
    public static bool AreEquivalentUsingCulture(string text1, string text2)
    {
        var comparer = StringComparer.Create(
            CultureInfo.GetCultureInfo("pt-BR"),
            CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
        );

        return comparer.Equals(text1, text2);
    }
}