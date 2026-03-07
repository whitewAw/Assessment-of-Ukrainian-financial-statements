using AFS.Core.Models;

namespace AFS.Core.Services.DataCalculations
{
    public class WorkingCapitalTurnoverTimeChart
    {
        FactorsAffectingTurnoverOfWorkingCapital? FactorsAffectingTurnoverOfWorkingCapital { get; set; }
        AFSModel model { get; set; } = new();

        public WorkingCapitalTurnoverTimeChart(AFSModel model) => Init(model);
        private void Init(AFSModel model)
        {
            FactorsAffectingTurnoverOfWorkingCapital = new(model);
            this.model = model;
        }

        public List<ChartDateTimeItem> GetMoney() =>
            BuildTimeSeriesItems(
                FactorsAffectingTurnoverOfWorkingCapital?.Money.BaseYear,
                FactorsAffectingTurnoverOfWorkingCapital?.Money.CurrentYear);

        public List<ChartDateTimeItem> GetReceivables() =>
            BuildTimeSeriesItems(
                FactorsAffectingTurnoverOfWorkingCapital?.Receivables.BaseYear,
                FactorsAffectingTurnoverOfWorkingCapital?.Receivables.CurrentYear);

        public List<ChartDateTimeItem> GetMaterialValues() =>
            BuildTimeSeriesItems(
                FactorsAffectingTurnoverOfWorkingCapital?.MaterialValues.BaseYear,
                FactorsAffectingTurnoverOfWorkingCapital?.MaterialValues.CurrentYear);

        private List<ChartDateTimeItem> BuildTimeSeriesItems(double? baseYearValue, double? currentYearValue)
        {
            List<ChartDateTimeItem> items = [];

            var baseVal = baseYearValue.GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(baseVal))
            {
                items.Add(new ChartDateTimeItem { Date = new DateTime(model.BaseYear, 1, 1), Value = baseVal });
            }

            var currentVal = currentYearValue.GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(currentVal))
            {
                items.Add(new ChartDateTimeItem { Date = new DateTime(model.CurrentYear, 12, 31), Value = currentVal });
            }

            return items;
        }
    }
}
