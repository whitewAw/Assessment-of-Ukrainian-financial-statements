using AFS.Core.Models;

namespace AFS.Core.Services.DataCalculations
{
    public class CompositionOfAssetsChart
    {
        CharacteristicsOfCapital? CharacteristicsOfCapital { get; set; }

        public CompositionOfAssetsChart(AFSModel model) => Init(model);

        private void Init(AFSModel model)
        {
            CharacteristicsOfCapital = new(model);
        }

        public List<ChartDataItem> GetDataItem(bool baseYear, bool begin)
        {
            List<ChartDataItem> assets = [];

            var nonCurrentImmobilizedAssetsValue = GetNonCurrentImmobilizedFunds(baseYear, begin).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(nonCurrentImmobilizedAssetsValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "NonCurrentImmobilizedAssets",
                    Value = nonCurrentImmobilizedAssetsValue
                });
            }
            var tangibleCurrentAssetsValue = GetTangibleCurrentAssets(baseYear, begin).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(tangibleCurrentAssetsValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "TangibleCurrentAssets",
                    Value = tangibleCurrentAssetsValue
                });
            }
            var accountsReceivableValue = GetAccountsReceivable(baseYear, begin).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(accountsReceivableValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "AccountsReceivable",
                    Value = accountsReceivableValue
                });
            }
            var cashCurrentFinancialInvestmentsValue = GetCashCurrentFinancialInvestments(baseYear, begin).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(cashCurrentFinancialInvestmentsValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "CashCurrentFinancialInvestments",
                    Value = cashCurrentFinancialInvestmentsValue
                });
            }
            var otherCurrentAssetsValue = GetOtherCurrentAssets(baseYear, begin).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(otherCurrentAssetsValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "OtherCurrentAssets",
                    Value = otherCurrentAssetsValue
                });
            }
            var nonCurrentAssetsHeldForSaleValue = GetNonCurrentAssetsHeldForSale(baseYear, begin).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(nonCurrentAssetsHeldForSaleValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "NonCurrentAssetsHeldForSale",
                    Value = nonCurrentAssetsHeldForSaleValue
                });
            }
            var futureExpensesValue = GetFutureExpenses(baseYear, begin).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(futureExpensesValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "FutureExpenses",
                    Value = futureExpensesValue
                });
            }

            return assets.OrderByDescending(item => item.Value).ToList();
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
