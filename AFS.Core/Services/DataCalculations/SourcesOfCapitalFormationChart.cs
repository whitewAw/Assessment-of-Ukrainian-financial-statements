using AFS.Core.Models;

namespace AFS.Core.Services.DataCalculations
{
    public class SourcesOfCapitalFormationChart
    {
        SourcesOfCapitalFormation? SourcesOfCapitalFormation { get; set; }

        public SourcesOfCapitalFormationChart(AFSModel model) => Init(model);
        private void Init(AFSModel model) => SourcesOfCapitalFormation = new(model);
        public List<ChartDataItem> GetDataItem(bool baseYear)
        {
            List<ChartDataItem> assets = [];

            var equityValue = GetEquity(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(equityValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "Equity",
                    Value = equityValue
                });
            }
            var longTermLiabilitiesValue = GetLongTermLiabilities(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(longTermLiabilitiesValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "LongTermLiabilities_",
                    Value = longTermLiabilitiesValue
                });
            }
            var shortTermLoansValue = GetShortTermLoans(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(shortTermLoansValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "ShortTermLoans",
                    Value = shortTermLoansValue
                });
            }
            var accountsPayableValue = GetAccountsPayable(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(accountsPayableValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "AccountsPayable",
                    Value = accountsPayableValue
                });
            }
            var otherCurrentLiabilitiesValue = GetOtherCurrentLiabilities(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(otherCurrentLiabilitiesValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "OtherCurrentLiabilities",
                    Value = otherCurrentLiabilitiesValue
                });
            }
            var liabilitiesRelatedToNonCurrentAssetsForSaleValue = GetLiabilitiesRelatedNonCurrentAssetsHeldForSale(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(liabilitiesRelatedToNonCurrentAssetsForSaleValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "LiabilitiesRelatedToNonCurrentAssetsForSale",
                    Value = liabilitiesRelatedToNonCurrentAssetsForSaleValue
                });
            }
            var futureIncomeValue = GetFutureIncome(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(futureIncomeValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "FutureIncome",
                    Value = futureIncomeValue
                });
            }


            return assets.OrderByDescending(item => item.Value).ToList();
        }

        private double? GetEquity(bool baseYear) =>
            baseYear
                ? SourcesOfCapitalFormation?.Equity.InPercentageOfAssetsBase.EndOfYear
                : SourcesOfCapitalFormation?.Equity.InPercentageOfAssetsCurrent.EndOfYear;

        private double? GetLongTermLiabilities(bool baseYear)
        {
            if (SourcesOfCapitalFormation == null) return 0;
            return baseYear
                ? SourcesOfCapitalFormation.LongTermLiabilities.Base.EndOfYear / SourcesOfCapitalFormation.TotalSourcesOfCapital.Base.EndOfYear * 100
                : SourcesOfCapitalFormation.LongTermLiabilities.Current.EndOfYear / SourcesOfCapitalFormation.TotalSourcesOfCapital.Current.EndOfYear * 100;
        }

        private double? GetShortTermLoans(bool baseYear)
        {
            if (SourcesOfCapitalFormation == null) return 0;
            return baseYear
                ? SourcesOfCapitalFormation.ShortTermLoans.Base.EndOfYear / SourcesOfCapitalFormation.TotalSourcesOfCapital.Base.EndOfYear * 100
                : SourcesOfCapitalFormation.ShortTermLoans.Current.EndOfYear / SourcesOfCapitalFormation.TotalSourcesOfCapital.Current.EndOfYear * 100;
        }

        private double? GetAccountsPayable(bool baseYear)
        {
            if (SourcesOfCapitalFormation == null) return 0;
            return baseYear
                ? SourcesOfCapitalFormation.AccountsPayable.Base.EndOfYear / SourcesOfCapitalFormation.TotalSourcesOfCapital.Base.EndOfYear * 100
                : SourcesOfCapitalFormation.AccountsPayable.Current.EndOfYear / SourcesOfCapitalFormation.TotalSourcesOfCapital.Current.EndOfYear * 100;
        }

        private double? GetOtherCurrentLiabilities(bool baseYear)
        {
            if (SourcesOfCapitalFormation == null) return 0;
            return baseYear
                ? SourcesOfCapitalFormation.OtherCurrentLiabilities.Base.EndOfYear / SourcesOfCapitalFormation.TotalSourcesOfCapital.Base.EndOfYear * 100
                : SourcesOfCapitalFormation.OtherCurrentLiabilities.Current.EndOfYear / SourcesOfCapitalFormation.TotalSourcesOfCapital.Current.EndOfYear * 100;
        }

        private double? GetLiabilitiesRelatedNonCurrentAssetsHeldForSale(bool baseYear)
        {
            if (SourcesOfCapitalFormation == null) return 0;
            return baseYear
                ? SourcesOfCapitalFormation.LiabilitiesRelatedNonCurrentAssetsHeldForSale.Base.EndOfYear / SourcesOfCapitalFormation.TotalSourcesOfCapital.Base.EndOfYear * 100
                : SourcesOfCapitalFormation.LiabilitiesRelatedNonCurrentAssetsHeldForSale.Current.EndOfYear / SourcesOfCapitalFormation.TotalSourcesOfCapital.Current.EndOfYear * 100;
        }

        private double? GetFutureIncome(bool baseYear) =>
            baseYear
                ? SourcesOfCapitalFormation?.FutureIncome.InPercentageOfAssetsBase.EndOfYear
                : SourcesOfCapitalFormation?.FutureIncome.InPercentageOfAssetsCurrent.EndOfYear;
    }
}
