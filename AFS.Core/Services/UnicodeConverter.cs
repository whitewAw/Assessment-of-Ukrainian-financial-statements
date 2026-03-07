using System.Globalization;
using System.Text.RegularExpressions;

namespace AFS.Core.Services
{
    internal partial class UnicodeConverter
    {
        [GeneratedRegex(@"\\[uU]([0-9A-F]{4})")]
        private static partial Regex UnicodeEscapeRegex();

        internal static string UnicodeEscapesIntoUnicodeCharacters(string str)
        {
            return UnicodeEscapeRegex().Replace(str, match =>
                ((char)int.Parse(match.Value.AsSpan(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture)).ToString());
        }
    }
}
