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

            AddIfValid(assets, "Equity", GetEquity(baseYear));
            AddIfValid(assets, "LongTermLiabilities_", GetLongTermLiabilities(baseYear));
            AddIfValid(assets, "ShortTermLoans", GetShortTermLoans(baseYear));
            AddIfValid(assets, "AccountsPayable", GetAccountsPayable(baseYear));
            AddIfValid(assets, "OtherCurrentLiabilities", GetOtherCurrentLiabilities(baseYear));
            AddIfValid(assets, "LiabilitiesRelatedToNonCurrentAssetsForSale", GetLiabilitiesRelatedNonCurrentAssetsHeldForSale(baseYear));
            AddIfValid(assets, "FutureIncome", GetFutureIncome(baseYear));

            return assets.OrderByDescending(item => item.Value).ToList();
        }

        private static void AddIfValid(List<ChartDataItem> assets, string item, double? value)
        {
            var val = value.GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(val))
            {
                assets.Add(new ChartDataItem { Item = item, Value = val });
            }
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
