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

            var withBuyersOrSuppliersValue = GetWithBuyersOrSuppliers(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(withBuyersOrSuppliersValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "WithBuyersOrSuppliers",
                    Value = withBuyersOrSuppliersValue
                });
            }
            var withLongTermLiabilitiesValue = GetWithLongTermLiabilities(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(withLongTermLiabilitiesValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "WithLongTermLiabilities",
                    Value = withLongTermLiabilitiesValue
                });
            }
            var forBillsValue = GetForBills(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(forBillsValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "ForBills",
                    Value = forBillsValue
                });
            }
            var fromInsuranceValue = GetFromInsurance(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(fromInsuranceValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "FromInsurance",
                    Value = fromInsuranceValue
                });
            }
            var withBudgetAndExtraBudgetaryFundsValue = GetWithBudgetAndExtraBudgetaryFunds(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(withBudgetAndExtraBudgetaryFundsValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "WithBudgetAndExtraBudgetaryFunds",
                    Value = withBudgetAndExtraBudgetaryFundsValue
                });
            }
            var withAccruedIncomeValue = GetWithAccruedIncome(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(withAccruedIncomeValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "WithAccruedIncome",
                    Value = withAccruedIncomeValue
                });
            }
            var withPayrollValue = GetWithPayroll(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(withPayrollValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "WithPayroll",
                    Value = withPayrollValue
                });
            }
            var withAdvancesValue = GetWithAdvances(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(withAdvancesValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "WithAdvances",
                    Value = withAdvancesValue
                });
            }
            var withParticipantsValue = GetWithParticipants(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(withParticipantsValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "WithParticipants",
                    Value = withParticipantsValue
                });
            }
            var withInternalCashSettlementsValue = GetWithInternalCashSettlements(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(withInternalCashSettlementsValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "WithInternalCashSettlements",
                    Value = withInternalCashSettlementsValue
                });
            }
            var withOtherValue = GetWithOther(baseYear).GetValueOrDefault(0);
            if (!AFSConstraints.IsZeroOrInvalid(withOtherValue))
            {
                assets.Add(new ChartDataItem
                {
                    Item = "WithOther",
                    Value = withOtherValue
                });
            }

            return assets.OrderByDescending(item => item.Value).ToList();
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
