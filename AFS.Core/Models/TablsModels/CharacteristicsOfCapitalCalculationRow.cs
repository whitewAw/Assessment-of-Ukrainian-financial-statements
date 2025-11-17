namespace AFS.Core.Models.TablsModels
{
    public class CharacteristicsOfCapitalCalculationRow
    {
        private double _beginningOfyear;
        private double _endOfYear;

        public double BeginningOfyear
        {
            get => _beginningOfyear;
            set => _beginningOfyear = SafeValue(value);
        }

        public double EndOfYear
        {
            get => _endOfYear;
            set => _endOfYear = SafeValue(value);
        }

        public double Deviations => SafeValue(EndOfYear - BeginningOfyear);

        public double GrowthRate
        {
            get
            {
                if (BeginningOfyear == 0)
                {
                    return EndOfYear == 0 ? 0 : 100; // If both zero = 0%, if only beginning zero = 100%
                }
                return SafeValue((EndOfYear / BeginningOfyear) * 100);
            }
        }

        /// <summary>
        /// Ensures the value is safe for JSON serialization (no NaN or Infinity)
        /// </summary>
        private static double SafeValue(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                return 0;
            }
            return value;
        }
    }
}
