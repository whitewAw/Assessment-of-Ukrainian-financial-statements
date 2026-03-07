namespace AFS.Core.Models
{
    public class AFSConstraints
    {
        public string FileExtension { get; private set; } = ".json";
        public int MaxFileSize { get; private set; } = 10 * 1024;
        public static int MinYear { get; private set; } = 2014;
        public static int MaxYear { get; private set; } = DateTime.Now.AddYears(-1).Year;
        public static string LangCultureName { get; private set; } = "langCulture";
        public static string ModelJsonName { get; private set; } = "modelJson";
        public static int DurationOAnalyzedPeriod { get; private set; } = 360;

        /// <summary>
        /// Tolerance for floating-point zero comparisons.
        /// Using a small epsilon to handle floating-point precision issues.
        /// </summary>
        private const double ZeroTolerance = 1e-10;

        /// <summary>
        /// Checks if a double value is effectively zero (within tolerance) or invalid (NaN/Infinity).
        /// </summary>
        public static bool IsZeroOrInvalid(double value) =>
            Math.Abs(value) < ZeroTolerance || double.IsNaN(value) || double.IsInfinity(value);

        /// <summary>
        /// Checks if a double value is effectively zero (within tolerance).
        /// </summary>
        public static bool IsEffectivelyZero(double value) =>
            Math.Abs(value) < ZeroTolerance;

        /// <summary>
        /// Safe division that returns 0 when denominator is zero or invalid.
        /// </summary>
        public static double SafeDivide(double numerator, double denominator, double defaultValue = 0) =>
            IsZeroOrInvalid(denominator) ? defaultValue : numerator / denominator;

        /// <summary>
        /// Calculates growth rate percentage safely.
        /// Returns 0 if both values are zero, 100 if only base is zero.
        /// </summary>
        public static double CalculateGrowthRate(double baseValue, double currentValue)
        {
            if (IsEffectivelyZero(baseValue))
            {
                return IsEffectivelyZero(currentValue) ? 0 : 100;
            }
            return (currentValue - baseValue) / baseValue * 100;
        }

        /// <summary>
        /// Gets the language name for AI prompts based on the current UI culture.
        /// Supports: Ukrainian, English, German, Spanish, French, Russian.
        /// </summary>
        public static string GetLanguageInstruction()
        {
            var cultureName = System.Globalization.CultureInfo.CurrentUICulture.Name;

            return cultureName[..2].ToLowerInvariant() switch
            {
                "uk" => "Ukrainian",
                "en" => "English",
                "de" => "German",
                "es" => "Spanish",
                "fr" => "French",
                "ru" => "Russian",
                _ => "English"
            };
        }

        public static double RoundStat(double value, int digits = 1) => Math.Round(value, digits, MidpointRounding.ToEven);

        public static string RoundStrStat(double value, int digits = 1)
        {
            if (IsZeroOrInvalid(value))
            {
                return string.Empty;
            }
            string mask = digits switch
            {
                1 => "0.0",
                2 => "0.00",
                3 => "0.000",
                4 => "0.0000",
                _ => throw new ArgumentException("Invalid digits value", nameof(digits)),
            };
            return RoundStat(value, digits).ToString(mask, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static double Round(double value, int digits = 1) => RoundStat(value, digits);
        public static string RoundStr(double value, int digits = 1) => RoundStrStat(value, digits);
    }
}
