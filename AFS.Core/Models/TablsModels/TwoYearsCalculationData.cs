namespace AFS.Core.Models.TablsModels
{
    public class TwoYearsCalculationData
    {
        private double _baseYear;
        private double _currentYear;

        public string? Number { get; set; }

        public double BaseYear
        {
            get => _baseYear;
            set => _baseYear = SafeValue(value);
        }

        public double CurrentYear
        {
            get => _currentYear;
            set => _currentYear = SafeValue(value);
        }

        public double Deviations => SafeValue(CurrentYear - BaseYear);

        public double GrowthRate
        {
            get
            {
                if (BaseYear == 0)
                {
                    return CurrentYear == 0 ? 0 : 100; // If both zero = 0%, if only base zero = 100%
                }
                return SafeValue((CurrentYear / BaseYear) * 100);
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
