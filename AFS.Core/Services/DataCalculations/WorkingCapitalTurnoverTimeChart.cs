using AFS.Core.Models;

namespace AFS.Core.Services.DataCalculations
{
    public class WorkingCapitalTurnoverTimeChart
    {
        FactorsAffectingTurnoverOfWorkingCapital? FactorsAffectingTurnoverOfWorkingCapital { get; set; }
        AfsModel model { get; set; } = new();

        public WorkingCapitalTurnoverTimeChart(AfsModel model) => Init(model);
        private void Init(AfsModel model)
        {
            FactorsAffectingTurnoverOfWorkingCapital = new(model);
            this.model = model;
        }

        public IReadOnlyList<ChartDateTimeItem> GetMoney() =>
            BuildTimeSeriesItems(
                FactorsAffectingTurnoverOfWorkingCapital?.Money.BaseYear,
                FactorsAffectingTurnoverOfWorkingCapital?.Money.CurrentYear);

        public IReadOnlyList<ChartDateTimeItem> GetReceivables() =>
            BuildTimeSeriesItems(
                FactorsAffectingTurnoverOfWorkingCapital?.Receivables.BaseYear,
                FactorsAffectingTurnoverOfWorkingCapital?.Receivables.CurrentYear);

        public IReadOnlyList<ChartDateTimeItem> GetMaterialValues() =>
            BuildTimeSeriesItems(
                FactorsAffectingTurnoverOfWorkingCapital?.MaterialValues.BaseYear,
                FactorsAffectingTurnoverOfWorkingCapital?.MaterialValues.CurrentYear);

        private List<ChartDateTimeItem> BuildTimeSeriesItems(double? baseYearValue, double? currentYearValue)
        {
            List<ChartDateTimeItem> items = [];

            var baseVal = baseYearValue.GetValueOrDefault(0);
            if (!AfsConstraints.IsZeroOrInvalid(baseVal))
            {
                items.Add(new ChartDateTimeItem { Date = new DateTime(model.BaseYear, 1, 1, 0, 0, 0, DateTimeKind.Utc), Value = baseVal });
            }

            var currentVal = currentYearValue.GetValueOrDefault(0);
            if (!AfsConstraints.IsZeroOrInvalid(currentVal))
            {
                items.Add(new ChartDateTimeItem { Date = new DateTime(model.CurrentYear, 12, 31, 0, 0, 0, DateTimeKind.Utc), Value = currentVal });
            }

            return items;
        }
    }
}
