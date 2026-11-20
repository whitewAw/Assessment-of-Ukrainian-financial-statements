using AFS.Core.Interfaces;

namespace AFS.Core.Models.TablsModels
{
    public class TwoYearsCalculationData : IHasBaseCurrentYear
    {
        private double _baseYear;
        private double _currentYear;

        public string? Number { get; set; }
        public bool PreserveUnavailable { get; init; }

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
                if (PreserveUnavailable)
                {
                    return SafeValue(AfsConstraints.SafeRatio(CurrentYear, BaseYear) * 100);
                }
                if (AfsConstraints.IsEffectivelyZero(BaseYear))
                {
                    return AfsConstraints.IsEffectivelyZero(CurrentYear) ? 0 : 100;
                }
                return SafeValue((CurrentYear / BaseYear) * 100);
            }
        }

        private double SafeValue(double value)
        {
            if (double.IsFinite(value))
            {
                return value;
            }
            return PreserveUnavailable ? double.NaN : 0;
        }
    }
}
