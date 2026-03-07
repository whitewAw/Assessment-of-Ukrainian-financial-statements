using System.Globalization;
using System.Text.RegularExpressions;

namespace AFS.Core.Services
{
    internal partial class UnicodeConverter
    {
        [GeneratedRegex(@"\\[uU][0-9A-Fa-f]{4}", RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
        private static partial Regex UnicodeEscapeRegex();

        internal static string UnicodeEscapesIntoUnicodeCharacters(string str)
        {
            return UnicodeEscapeRegex().Replace(str, match =>
                ((char)int.Parse(match.Value.AsSpan(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture)).ToString());
        }
    }
}
