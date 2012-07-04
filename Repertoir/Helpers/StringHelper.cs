using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Repertoir.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Replace "accented" characters in a string with their equivalent English character
        /// http://code.commongroove.com/2011/04/29/c-string-extension-to-replace-accented-characters/
        /// </summary>
        /// <param name="source">String with characters like "ö", "è" and "ñ"</param>
        /// <returns>String with characters like "o", "e" and "n"</returns>
        public static string Unaccentize(this string source)
        {
            string sourceInFormD = source.Normalize(NormalizationForm.FormD);

            var output = new StringBuilder();
            foreach (char c in sourceInFormD)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                    output.Append(c);
            }

            return (output.ToString().Normalize(NormalizationForm.FormC));
        }

        public static string Slugify(this string text)
        {
            // Converti tout en minuscule sans accents
            string result = text.Unaccentize().ToLower();

            // Remplace tout signe de ponctuation par un espace
            // http://stackoverflow.com/questions/3973602/regex-match-any-punctuation-character-except-and
            result = Regex.Replace(result, @"[\p{P}]", " ");

            // Conserve uniquement les lettres, les chiffres et les espaces
            // http://predicatet.blogspot.com/2009/04/improved-c-slug-generator-or-how-to.html
            result = Regex.Replace(result, @"[^a-z0-9\s-]", "");

            // Remplace les espaces par un seul tiret
            result = Regex.Replace(result.Trim(), @"\s+", "-");

            // Renvoie le résultat
            return result;
        }

    }
}