using AFS.Core.Interfaces;

namespace AFS.Core.Models.TablsModels
{
    public class CharacteristicsOfCapitalCalculationRow : IHasBeginEnd
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
                if (AFSConstraints.IsEffectivelyZero(BeginningOfyear))
                {
                    return AFSConstraints.IsEffectivelyZero(EndOfYear) ? 0 : 100;
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
