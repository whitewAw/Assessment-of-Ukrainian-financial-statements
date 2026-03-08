using AFS.Core.Models;

namespace AFS.Core.Services.DataCalculations
{
    public class CompositionOfAssetsChart
    {
        CharacteristicsOfCapital? CharacteristicsOfCapital { get; set; }

        public CompositionOfAssetsChart(AfsModel model) => Init(model);

        private void Init(AfsModel model)
        {
            CharacteristicsOfCapital = new(model);
        }

        public IReadOnlyList<ChartDataItem> GetDataItem(bool baseYear, bool begin)
        {
            List<ChartDataItem> assets = [];

            AddIfValid(assets, "NonCurrentImmobilizedAssets", GetNonCurrentImmobilizedFunds(baseYear, begin));
            AddIfValid(assets, "TangibleCurrentAssets", GetTangibleCurrentAssets(baseYear, begin));
            AddIfValid(assets, "AccountsReceivable", GetAccountsReceivable(baseYear, begin));
            AddIfValid(assets, "CashCurrentFinancialInvestments", GetCashCurrentFinancialInvestments(baseYear, begin));
            AddIfValid(assets, "OtherCurrentAssets", GetOtherCurrentAssets(baseYear, begin));
            AddIfValid(assets, "NonCurrentAssetsHeldForSale", GetNonCurrentAssetsHeldForSale(baseYear, begin));
            AddIfValid(assets, "FutureExpenses", GetFutureExpenses(baseYear, begin));

            return assets.OrderByDescending(item => item.Value).ToList();
        }

        private static void AddIfValid(List<ChartDataItem> assets, string item, double? value)
        {
            var val = value.GetValueOrDefault(0);
            if (!AfsConstraints.IsZeroOrInvalid(val))
            {
                assets.Add(new ChartDataItem { Item = item, Value = val });
            }
        }

        public double? GetNonCurrentImmobilizedFunds(bool baseYear, bool begin) =>
            (begin, baseYear) switch
            {
                (true, true) => CharacteristicsOfCapital?.NonCurrentImmobilizedFunds.Base.BeginningOfyear,
                (false, true) => CharacteristicsOfCapital?.NonCurrentImmobilizedFunds.Base.EndOfYear,
                (true, false) => CharacteristicsOfCapital?.NonCurrentImmobilizedFunds.Current.BeginningOfyear,
                (false, false) => CharacteristicsOfCapital?.NonCurrentImmobilizedFunds.Current.EndOfYear,
            };

        public double? GetTangibleCurrentAssets(bool baseYear, bool begin) =>
            (begin, baseYear) switch
            {
                (true, true) => CharacteristicsOfCapital?.TangibleCurrentAssets.Base.BeginningOfyear,
                (false, true) => CharacteristicsOfCapital?.TangibleCurrentAssets.Base.EndOfYear,
                (true, false) => CharacteristicsOfCapital?.TangibleCurrentAssets.Current.BeginningOfyear,
                (false, false) => CharacteristicsOfCapital?.TangibleCurrentAssets.Current.EndOfYear,
            };

        public double? GetAccountsReceivable(bool baseYear, bool begin) =>
            (begin, baseYear) switch
            {
                (true, true) => CharacteristicsOfCapital?.AccountsReceivable.Base.BeginningOfyear,
                (false, true) => CharacteristicsOfCapital?.AccountsReceivable.Base.EndOfYear,
                (true, false) => CharacteristicsOfCapital?.AccountsReceivable.Current.BeginningOfyear,
                (false, false) => CharacteristicsOfCapital?.AccountsReceivable.Current.EndOfYear,
            };

        public double? GetCashCurrentFinancialInvestments(bool baseYear, bool begin) =>
            (begin, baseYear) switch
            {
                (true, true) => CharacteristicsOfCapital?.CashCurrentFinancialInvestments.Base.BeginningOfyear,
                (false, true) => CharacteristicsOfCapital?.CashCurrentFinancialInvestments.Base.EndOfYear,
                (true, false) => CharacteristicsOfCapital?.CashCurrentFinancialInvestments.Current.BeginningOfyear,
                (false, false) => CharacteristicsOfCapital?.CashCurrentFinancialInvestments.Current.EndOfYear,
            };

        public double? GetOtherCurrentAssets(bool baseYear, bool begin) =>
            (begin, baseYear) switch
            {
                (true, true) => CharacteristicsOfCapital?.OtherCurrentAssets.Base.BeginningOfyear,
                (false, true) => CharacteristicsOfCapital?.OtherCurrentAssets.Base.EndOfYear,
                (true, false) => CharacteristicsOfCapital?.OtherCurrentAssets.Current.BeginningOfyear,
                (false, false) => CharacteristicsOfCapital?.OtherCurrentAssets.Current.EndOfYear,
            };

        public double? GetNonCurrentAssetsHeldForSale(bool baseYear, bool begin) =>
            (begin, baseYear) switch
            {
                (true, true) => CharacteristicsOfCapital?.NonCurrentAssetsHeldForSale.Base.BeginningOfyear,
                (false, true) => CharacteristicsOfCapital?.NonCurrentAssetsHeldForSale.Base.EndOfYear,
                (true, false) => CharacteristicsOfCapital?.NonCurrentAssetsHeldForSale.Current.BeginningOfyear,
                (false, false) => CharacteristicsOfCapital?.NonCurrentAssetsHeldForSale.Current.EndOfYear,
            };

        public double? GetFutureExpenses(bool baseYear, bool begin) =>
            (begin, baseYear) switch
            {
                (true, true) => CharacteristicsOfCapital?.FutureExpenses.Base.BeginningOfyear,
                (false, true) => CharacteristicsOfCapital?.FutureExpenses.Base.EndOfYear,
                (true, false) => CharacteristicsOfCapital?.FutureExpenses.Current.BeginningOfyear,
                (false, false) => CharacteristicsOfCapital?.FutureExpenses.Current.EndOfYear,
            };
    }
}
