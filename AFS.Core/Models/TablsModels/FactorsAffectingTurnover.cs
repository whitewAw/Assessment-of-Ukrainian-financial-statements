namespace AFS.Core.Models.TablsModels
{
    public class FactorsAffectingTurnover
    {
        private double _baseYear;
        private double _currentYear;
        private double _adjustedIndicator;

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

        public double AdjustedIndicator
        {
            get => _adjustedIndicator;
            set => _adjustedIndicator = SafeValue(value);
        }

        public double Total => SafeValue(CurrentYear - BaseYear);

        public double DueToRevenue => SafeValue(AdjustedIndicator - BaseYear);

        public double DueToAverageBalances => SafeValue(CurrentYear - AdjustedIndicator);

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
