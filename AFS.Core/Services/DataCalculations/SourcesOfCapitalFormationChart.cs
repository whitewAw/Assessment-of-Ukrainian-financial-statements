using AFS.Core.Helpers;
using AFS.Core.Models;

namespace AFS.Core.Services.DataCalculations
{
    public class SourcesOfCapitalFormationChart
    {
        SourcesOfCapitalFormation? SourcesOfCapitalFormation { get; set; }

        public SourcesOfCapitalFormationChart(AfsModel model) => Init(model);
        private void Init(AfsModel model) => SourcesOfCapitalFormation = new(model);

        public IReadOnlyList<ChartDataItem> GetDataItem(bool baseYear)
        {
            List<ChartDataItem> assets = [];

            ChartDataHelper.AddIfValid(assets, "Equity", GetEquity(baseYear));
            ChartDataHelper.AddIfValid(assets, "LongTermLiabilities_", GetLongTermLiabilities(baseYear));
            ChartDataHelper.AddIfValid(assets, "ShortTermLoans", GetShortTermLoans(baseYear));
            ChartDataHelper.AddIfValid(assets, "AccountsPayable", GetAccountsPayable(baseYear));
            ChartDataHelper.AddIfValid(assets, "OtherCurrentLiabilities", GetOtherCurrentLiabilities(baseYear));
            ChartDataHelper.AddIfValid(assets, "LiabilitiesRelatedToNonCurrentAssetsForSale", GetLiabilitiesRelatedNonCurrentAssetsHeldForSale(baseYear));
            ChartDataHelper.AddIfValid(assets, "FutureIncome", GetFutureIncome(baseYear));

            return ChartDataHelper.SortDescending(assets);
        }

        private double? GetEquity(bool baseYear) =>
            baseYear
                ? SourcesOfCapitalFormation?.Equity.InPercentageOfAssetsBase.EndOfYear
                : SourcesOfCapitalFormation?.Equity.InPercentageOfAssetsCurrent.EndOfYear;

        private double? GetLongTermLiabilities(bool baseYear) =>
            baseYear
                ? ChartDataHelper.CalculatePercentage(
                    SourcesOfCapitalFormation?.LongTermLiabilities.Base.EndOfYear,
                    SourcesOfCapitalFormation?.TotalSourcesOfCapital.Base.EndOfYear)
                : ChartDataHelper.CalculatePercentage(
                    SourcesOfCapitalFormation?.LongTermLiabilities.Current.EndOfYear,
                    SourcesOfCapitalFormation?.TotalSourcesOfCapital.Current.EndOfYear);

        private double? GetShortTermLoans(bool baseYear) =>
            baseYear
                ? ChartDataHelper.CalculatePercentage(
                    SourcesOfCapitalFormation?.ShortTermLoans.Base.EndOfYear,
                    SourcesOfCapitalFormation?.TotalSourcesOfCapital.Base.EndOfYear)
                : ChartDataHelper.CalculatePercentage(
                    SourcesOfCapitalFormation?.ShortTermLoans.Current.EndOfYear,
                    SourcesOfCapitalFormation?.TotalSourcesOfCapital.Current.EndOfYear);

        private double? GetAccountsPayable(bool baseYear) =>
            baseYear
                ? ChartDataHelper.CalculatePercentage(
                    SourcesOfCapitalFormation?.AccountsPayable.Base.EndOfYear,
                    SourcesOfCapitalFormation?.TotalSourcesOfCapital.Base.EndOfYear)
                : ChartDataHelper.CalculatePercentage(
                    SourcesOfCapitalFormation?.AccountsPayable.Current.EndOfYear,
                    SourcesOfCapitalFormation?.TotalSourcesOfCapital.Current.EndOfYear);

        private double? GetOtherCurrentLiabilities(bool baseYear) =>
            baseYear
                ? ChartDataHelper.CalculatePercentage(
                    SourcesOfCapitalFormation?.OtherCurrentLiabilities.Base.EndOfYear,
                    SourcesOfCapitalFormation?.TotalSourcesOfCapital.Base.EndOfYear)
                : ChartDataHelper.CalculatePercentage(
                    SourcesOfCapitalFormation?.OtherCurrentLiabilities.Current.EndOfYear,
                    SourcesOfCapitalFormation?.TotalSourcesOfCapital.Current.EndOfYear);

        private double? GetLiabilitiesRelatedNonCurrentAssetsHeldForSale(bool baseYear) =>
            baseYear
                ? ChartDataHelper.CalculatePercentage(
                    SourcesOfCapitalFormation?.LiabilitiesRelatedNonCurrentAssetsHeldForSale.Base.EndOfYear,
                    SourcesOfCapitalFormation?.TotalSourcesOfCapital.Base.EndOfYear)
                : ChartDataHelper.CalculatePercentage(
                    SourcesOfCapitalFormation?.LiabilitiesRelatedNonCurrentAssetsHeldForSale.Current.EndOfYear,
                    SourcesOfCapitalFormation?.TotalSourcesOfCapital.Current.EndOfYear);

        private double? GetFutureIncome(bool baseYear) =>
            baseYear
                ? SourcesOfCapitalFormation?.FutureIncome.InPercentageOfAssetsBase.EndOfYear
                : SourcesOfCapitalFormation?.FutureIncome.InPercentageOfAssetsCurrent.EndOfYear;
    }
}
