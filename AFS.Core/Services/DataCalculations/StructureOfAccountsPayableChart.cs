using AFS.Core.Helpers;
using AFS.Core.Models;

namespace AFS.Core.Services.DataCalculations
{
    public class StructureOfAccountsPayableChart
    {
        AssessmentOfReceivableAndPayable? AssessmentOfReceivableAndPayable { get; set; }

        public StructureOfAccountsPayableChart(AfsModel model) => Init(model);
        private void Init(AfsModel model) => AssessmentOfReceivableAndPayable = new(model);

        public IReadOnlyList<ChartDataItem> GetDataItem(bool baseYear)
        {
            List<ChartDataItem> assets = [];

            ChartDataHelper.AddIfValid(assets, "WithBuyersOrSuppliers", GetWithBuyersOrSuppliers(baseYear));
            ChartDataHelper.AddIfValid(assets, "WithLongTermLiabilities", GetWithLongTermLiabilities(baseYear));
            ChartDataHelper.AddIfValid(assets, "ForBills", GetForBills(baseYear));
            ChartDataHelper.AddIfValid(assets, "FromInsurance", GetFromInsurance(baseYear));
            ChartDataHelper.AddIfValid(assets, "WithBudgetAndExtraBudgetaryFunds", GetWithBudgetAndExtraBudgetaryFunds(baseYear));
            ChartDataHelper.AddIfValid(assets, "WithAccruedIncome", GetWithAccruedIncome(baseYear));
            ChartDataHelper.AddIfValid(assets, "WithPayroll", GetWithPayroll(baseYear));
            ChartDataHelper.AddIfValid(assets, "WithAdvances", GetWithAdvances(baseYear));
            ChartDataHelper.AddIfValid(assets, "WithParticipants", GetWithParticipants(baseYear));
            ChartDataHelper.AddIfValid(assets, "WithInternalCashSettlements", GetWithInternalCashSettlements(baseYear));
            ChartDataHelper.AddIfValid(assets, "WithOther", GetWithOther(baseYear));

            return ChartDataHelper.SortDescending(assets);
        }

        private double? GetWithBuyersOrSuppliers(bool baseYear) =>
            baseYear
                ? ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithBuyersOrSuppliers.PayableBase,
                    AssessmentOfReceivableAndPayable?.Total.PayableBase)
                : ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithBuyersOrSuppliers.PayableCurrent,
                    AssessmentOfReceivableAndPayable?.Total.PayableCurrent);

        private double? GetWithLongTermLiabilities(bool baseYear) =>
            baseYear
                ? ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithLongTermLiabilities.PayableBase,
                    AssessmentOfReceivableAndPayable?.Total.PayableBase)
                : ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithLongTermLiabilities.PayableCurrent,
                    AssessmentOfReceivableAndPayable?.Total.PayableCurrent);

        private double? GetForBills(bool baseYear) =>
            baseYear
                ? ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.ForBills.PayableBase,
                    AssessmentOfReceivableAndPayable?.Total.PayableBase)
                : ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.ForBills.PayableCurrent,
                    AssessmentOfReceivableAndPayable?.Total.PayableCurrent);

        private double? GetFromInsurance(bool baseYear) =>
            baseYear
                ? ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.FromInsurance.PayableBase,
                    AssessmentOfReceivableAndPayable?.Total.PayableBase)
                : ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.FromInsurance.PayableCurrent,
                    AssessmentOfReceivableAndPayable?.Total.PayableCurrent);

        private double? GetWithBudgetAndExtraBudgetaryFunds(bool baseYear) =>
            baseYear
                ? ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithBudgetAndExtraBudgetaryFunds.PayableBase,
                    AssessmentOfReceivableAndPayable?.Total.PayableBase)
                : ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithBudgetAndExtraBudgetaryFunds.PayableCurrent,
                    AssessmentOfReceivableAndPayable?.Total.PayableCurrent);

        private double? GetWithAccruedIncome(bool baseYear) =>
            baseYear
                ? ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithAccruedIncome.PayableBase,
                    AssessmentOfReceivableAndPayable?.Total.PayableBase)
                : ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithAccruedIncome.PayableCurrent,
                    AssessmentOfReceivableAndPayable?.Total.PayableCurrent);

        private double? GetWithPayroll(bool baseYear) =>
            baseYear
                ? ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithPayroll.PayableBase,
                    AssessmentOfReceivableAndPayable?.Total.PayableBase)
                : ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithPayroll.PayableCurrent,
                    AssessmentOfReceivableAndPayable?.Total.PayableCurrent);

        private double? GetWithAdvances(bool baseYear) =>
            baseYear
                ? ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithAdvances.PayableBase,
                    AssessmentOfReceivableAndPayable?.Total.PayableBase)
                : ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithAdvances.PayableCurrent,
                    AssessmentOfReceivableAndPayable?.Total.PayableCurrent);

        private double? GetWithParticipants(bool baseYear) =>
            baseYear
                ? ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithParticipants.PayableBase,
                    AssessmentOfReceivableAndPayable?.Total.PayableBase)
                : ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithParticipants.PayableCurrent,
                    AssessmentOfReceivableAndPayable?.Total.PayableCurrent);

        private double? GetWithInternalCashSettlements(bool baseYear) =>
            baseYear
                ? ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithInternalCashSettlements.PayableBase,
                    AssessmentOfReceivableAndPayable?.Total.PayableBase)
                : ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithInternalCashSettlements.PayableCurrent,
                    AssessmentOfReceivableAndPayable?.Total.PayableCurrent);

        private double? GetWithOther(bool baseYear) =>
            baseYear
                ? ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithOther.PayableBase,
                    AssessmentOfReceivableAndPayable?.Total.PayableBase)
                : ChartDataHelper.CalculatePercentage(
                    AssessmentOfReceivableAndPayable?.WithOther.PayableCurrent,
                    AssessmentOfReceivableAndPayable?.Total.PayableCurrent);
    }
}
