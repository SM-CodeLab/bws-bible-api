using System.Globalization;
using System.Text;

namespace Bws.Bible.Core.Extensions;

/// <summary>
/// Extensions pour le type string
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Supprime une expression (chaine de caractères) à la fin d'une chaine de caractères.
    /// </summary>
    public static string TrimEnd(this string input, string expressionToRemove, StringComparison comparisonType)
    {
        if (input == null)
        {
            return string.Empty;
        }

        if (expressionToRemove != null && input.EndsWith(expressionToRemove, comparisonType))
        {
            return input.Substring(0, input.Length - expressionToRemove.Length);
        }
        else
        {
            return input;
        }
    }

    /// <summary>
    /// Compte le nombre de mots dans une chaine de caractères.
    /// </summary>
    public static int CountWords(this string input)
    {
        return input.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }

    /// <summary>
    /// Supprime les signes diacritiques sur les lettres (accents, cédilles etc.) d'une chaine de caractères
    /// </summary>
    public static string RemoveDiacritics(this string input)
    {
        var normalizedString = input.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}
