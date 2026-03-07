using AFS.Core.Models;

namespace AFS.Core.Services.DataCalculations
{
    public class StructureOfAccountsPayableChart
    {
        AssessmentOfReceivableAndPayable? AssessmentOfReceivableAndPayable { get; set; }

        public StructureOfAccountsPayableChart(AFSModel model) => Init(model);
        private void Init(AFSModel model) => AssessmentOfReceivableAndPayable = new(model);

        public List<ChartDataItem> GetDataItem(bool baseYear)
        {
            List<ChartDataItem> assets = [];

            AddIfValid(assets, "WithBuyersOrSuppliers", GetWithBuyersOrSuppliers(baseYear));
            AddIfValid(assets, "WithLongTermLiabilities", GetWithLongTermLiabilities(baseYear));
            AddIfValid(assets, "ForBills", GetForBills(baseYear));
            AddIfValid(assets, "FromInsurance", GetFromInsurance(baseYear));
            AddIfValid(assets, "WithBudgetAndExtraBudgetaryFunds", GetWithBudgetAndExtraBudgetaryFunds(baseYear));
            AddIfValid(assets, "WithAccruedIncome", GetWithAccruedIncome(baseYear));
            AddIfValid(assets, "WithPayroll", GetWithPayroll(baseYear));
            AddIfValid(assets, "WithAdvances", GetWithAdvances(baseYear));
            AddIfValid(assets, "WithParticipants", GetWithParticipants(baseYear));
            AddIfValid(assets, "WithInternalCashSettlements", GetWithInternalCashSettlements(baseYear));
            AddIfValid(assets, "WithOther", GetWithOther(baseYear));

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

        private double? GetWithBuyersOrSuppliers(bool baseYear)
        {
            if (AssessmentOfReceivableAndPayable == null) return 0;
            return baseYear
                ? AssessmentOfReceivableAndPayable.WithBuyersOrSuppliers.PayableBase / AssessmentOfReceivableAndPayable.Total.PayableBase * 100
                : AssessmentOfReceivableAndPayable.WithBuyersOrSuppliers.PayableCurrent / AssessmentOfReceivableAndPayable.Total.PayableCurrent * 100;
        }

        private double? GetWithLongTermLiabilities(bool baseYear)
        {
            if (AssessmentOfReceivableAndPayable == null) return 0;
            return baseYear
                ? AssessmentOfReceivableAndPayable.WithLongTermLiabilities.PayableBase / AssessmentOfReceivableAndPayable.Total.PayableBase * 100
                : AssessmentOfReceivableAndPayable.WithLongTermLiabilities.PayableCurrent / AssessmentOfReceivableAndPayable.Total.PayableCurrent * 100;
        }

        private double? GetForBills(bool baseYear)
        {
            if (AssessmentOfReceivableAndPayable == null) return 0;
            return baseYear
                ? AssessmentOfReceivableAndPayable.ForBills.PayableBase / AssessmentOfReceivableAndPayable.Total.PayableBase * 100
                : AssessmentOfReceivableAndPayable.ForBills.PayableCurrent / AssessmentOfReceivableAndPayable.Total.PayableCurrent * 100;
        }

        private double? GetFromInsurance(bool baseYear)
        {
            if (AssessmentOfReceivableAndPayable == null) return 0;
            return baseYear
                ? AssessmentOfReceivableAndPayable.FromInsurance.PayableBase / AssessmentOfReceivableAndPayable.Total.PayableBase * 100
                : AssessmentOfReceivableAndPayable.FromInsurance.PayableCurrent / AssessmentOfReceivableAndPayable.Total.PayableCurrent * 100;
        }

        private double? GetWithBudgetAndExtraBudgetaryFunds(bool baseYear)
        {
            if (AssessmentOfReceivableAndPayable == null) return 0;
            return baseYear
                ? AssessmentOfReceivableAndPayable.WithBudgetAndExtraBudgetaryFunds.PayableBase / AssessmentOfReceivableAndPayable.Total.PayableBase * 100
                : AssessmentOfReceivableAndPayable.WithBudgetAndExtraBudgetaryFunds.PayableCurrent / AssessmentOfReceivableAndPayable.Total.PayableCurrent * 100;
        }

        private double? GetWithAccruedIncome(bool baseYear)
        {
            if (AssessmentOfReceivableAndPayable == null) return 0;
            return baseYear
                ? AssessmentOfReceivableAndPayable.WithAccruedIncome.PayableBase / AssessmentOfReceivableAndPayable.Total.PayableBase * 100
                : AssessmentOfReceivableAndPayable.WithAccruedIncome.PayableCurrent / AssessmentOfReceivableAndPayable.Total.PayableCurrent * 100;
        }

        private double? GetWithPayroll(bool baseYear)
        {
            if (AssessmentOfReceivableAndPayable == null) return 0;
            return baseYear
                ? AssessmentOfReceivableAndPayable.WithPayroll.PayableBase / AssessmentOfReceivableAndPayable.Total.PayableBase * 100
                : AssessmentOfReceivableAndPayable.WithPayroll.PayableCurrent / AssessmentOfReceivableAndPayable.Total.PayableCurrent * 100;
        }

        private double? GetWithAdvances(bool baseYear)
        {
            if (AssessmentOfReceivableAndPayable == null) return 0;
            return baseYear
                ? AssessmentOfReceivableAndPayable.WithAdvances.PayableBase / AssessmentOfReceivableAndPayable.Total.PayableBase * 100
                : AssessmentOfReceivableAndPayable.WithAdvances.PayableCurrent / AssessmentOfReceivableAndPayable.Total.PayableCurrent * 100;
        }

        private double? GetWithParticipants(bool baseYear)
        {
            if (AssessmentOfReceivableAndPayable == null) return 0;
            return baseYear
                ? AssessmentOfReceivableAndPayable.WithParticipants.PayableBase / AssessmentOfReceivableAndPayable.Total.PayableBase * 100
                : AssessmentOfReceivableAndPayable.WithParticipants.PayableCurrent / AssessmentOfReceivableAndPayable.Total.PayableCurrent * 100;
        }

        private double? GetWithInternalCashSettlements(bool baseYear)
        {
            if (AssessmentOfReceivableAndPayable == null) return 0;
            return baseYear
                ? AssessmentOfReceivableAndPayable.WithInternalCashSettlements.PayableBase / AssessmentOfReceivableAndPayable.Total.PayableBase * 100
                : AssessmentOfReceivableAndPayable.WithInternalCashSettlements.PayableCurrent / AssessmentOfReceivableAndPayable.Total.PayableCurrent * 100;
        }

        private double? GetWithOther(bool baseYear)
        {
            if (AssessmentOfReceivableAndPayable == null) return 0;
            return baseYear
                ? AssessmentOfReceivableAndPayable.WithOther.PayableBase / AssessmentOfReceivableAndPayable.Total.PayableBase * 100
                : AssessmentOfReceivableAndPayable.WithOther.PayableCurrent / AssessmentOfReceivableAndPayable.Total.PayableCurrent * 100;
        }
    }
}
