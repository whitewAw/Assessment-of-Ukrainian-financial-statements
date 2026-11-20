using AFS.Core.Models;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations
{
    public class IndicatorsOfBusinessActivity
    {
        public TwoYearsCalculationData GrossProfitPerSoldProducts { get; } = NewMetric();
        public TwoYearsCalculationData CoefficientOfBusinessActivity { get; } = NewMetric();
        public TwoYearsCalculationData CoefficientOfEfficiencFinancialResources { get; } = NewMetric();
        public TwoYearsCalculationData CoefficientOfOwnFundsUtilization { get; } = NewMetric();
        public TwoYearsCalculationData ProfitabilityRatioOfEnterprise { get; } = NewMetric();
        public TwoYearsCalculationData ProductivityOfOneAverageEmployee { get; } = NewMetric();
        public TwoYearsCalculationData ReturnOnFixedAssets { get; } = NewMetric();
        public TwoYearsCalculationData RevolutionsFromReceivables { get; } = NewMetric();
        public TwoYearsCalculationData ReceivablesTurnover { get; } = NewMetric();
        public TwoYearsCalculationData TurnoverOfInventoriesRevolutions { get; } = NewMetric();
        public TwoYearsCalculationData TurnoverOfInventoriesDays { get; } = NewMetric();
        public TwoYearsCalculationData OperatingCycleDuration { get; } = NewMetric();
        public TwoYearsCalculationData NumberOfRevolutionsOfCurrentAssets { get; } = NewMetric();
        public TwoYearsCalculationData TurnoverOfWorkingCapital { get; } = NewMetric();
        public TwoYearsCalculationData TurnoverOfEquity { get; } = NewMetric();
        public TwoYearsCalculationData TurnoverOfTotalCapital { get; } = NewMetric();
        public TwoYearsCalculationData CoefficientOfStabilityOfEconomicGrowth { get; } = NewMetric();
        public TwoYearsCalculationData ReturnOnEquityPeriod { get; } = NewMetric();

        private static TwoYearsCalculationData NewMetric() => new() { PreserveUnavailable = true };

        public IndicatorsOfBusinessActivity(AfsModel model) => Init(model);
        private void Init(AfsModel model)
        {
            SourcesOfCapitalFormation sOCF = new(model);
            FactorAnalysisOfFixedAssets fAOFA = new(model);
            IndicatorsOfTurnoverOfCurrentAssets iOTOCA = new(model);
            CharacteristicsOfCapital cOC = new(model);

            GrossProfitPerSoldProductsInit(model);
            CoefficientOfBusinessActivityInit(model);
            CoefficientOfEfficiencFinancialResourcesInit(model);
            CoefficientOfOwnFundsUtilizationInit(model, sOCF);
            ProfitabilityRatioOfEnterpriseInit();
            ProductivityOfOneAverageEmployeeInit(fAOFA);
            ReturnOnFixedAssetsInit(fAOFA);
            RevolutionsFromReceivablesInit(iOTOCA);
            ReceivablesTurnoverInit(iOTOCA);
            TurnoverOfInventoriesRevolutionsInit(model, cOC);
            TurnoverOfInventoriesDaysInit();
            OperatingCycleDurationInit();
            NumberOfRevolutionsOfCurrentAssetsInit(iOTOCA);
            TurnoverOfWorkingCapitalInit(iOTOCA);
            TurnoverOfEquityInit(model, sOCF);
            TurnoverOfTotalCapitalInit(model, sOCF);
            CoefficientOfStabilityOfEconomicGrowthInit(model, sOCF);
            ReturnOnEquityPeriodInit();
        }
        private void GrossProfitPerSoldProductsInit(AfsModel model)
        {
            GrossProfitPerSoldProducts.Number = "1.";
            GrossProfitPerSoldProducts.BaseYear = AfsConstraints.SafeRatio(model.F2Base.GetGrossResultCurrent(), model.F2Base.F2000.Current);
            GrossProfitPerSoldProducts.CurrentYear = AfsConstraints.SafeRatio(model.F2Current.GetGrossResultCurrent(), model.F2Current.F2000.Current);
        }
        private void CoefficientOfBusinessActivityInit(AfsModel model)
        {
            CoefficientOfBusinessActivity.Number = "2.";
            CoefficientOfBusinessActivity.BaseYear = model.F2Base.F2000.Current / ((model.F1Base.GetF1900Begin() + model.F1Base.GetF1900End()) / 2);
            CoefficientOfBusinessActivity.CurrentYear = model.F2Current.F2000.Current / ((model.F1Current.GetF1900Begin() + model.F1Current.GetF1900End()) / 2);
        }
        private void CoefficientOfEfficiencFinancialResourcesInit(AfsModel model)
        {
            CoefficientOfEfficiencFinancialResources.Number = "3.";
            CoefficientOfEfficiencFinancialResources.BaseYear = model.F2Base.GetPreTaxResultCurrent() / ((model.F1Base.GetF1900Begin() + model.F1Base.GetF1900End()) / 2);
            CoefficientOfEfficiencFinancialResources.CurrentYear = model.F2Current.GetPreTaxResultCurrent() / ((model.F1Current.GetF1900Begin() + model.F1Current.GetF1900End()) / 2);
        }
        private void CoefficientOfOwnFundsUtilizationInit(AfsModel model, SourcesOfCapitalFormation sOCF)
        {
            CoefficientOfOwnFundsUtilization.Number = "4.";
            CoefficientOfOwnFundsUtilization.BaseYear = model.F2Base.GetF2465Current() / ((sOCF.Equity.Base.BeginningOfyear + sOCF.Equity.Base.EndOfYear) / 2);
            CoefficientOfOwnFundsUtilization.CurrentYear = model.F2Current.GetF2465Current() / ((sOCF.Equity.Current.BeginningOfyear + sOCF.Equity.Current.EndOfYear) / 2);
        }
        private void ProfitabilityRatioOfEnterpriseInit()
        {
            ProfitabilityRatioOfEnterprise.Number = "5.";
            ProfitabilityRatioOfEnterprise.BaseYear = CoefficientOfOwnFundsUtilization.BaseYear / 0.19;
            ProfitabilityRatioOfEnterprise.CurrentYear = CoefficientOfOwnFundsUtilization.CurrentYear / 0.19;
        }
        private void ProductivityOfOneAverageEmployeeInit(FactorAnalysisOfFixedAssets fAOFA)
        {
            ProductivityOfOneAverageEmployee.Number = "6.";
            ProductivityOfOneAverageEmployee.BaseYear = fAOFA.ProductivityOfOneAverageEmployee.BaseYear;
            ProductivityOfOneAverageEmployee.CurrentYear = fAOFA.ProductivityOfOneAverageEmployee.CurrentYear;
        }
        private void ReturnOnFixedAssetsInit(FactorAnalysisOfFixedAssets fAOFA)
        {
            ReturnOnFixedAssets.Number = "7.";
            ReturnOnFixedAssets.BaseYear = fAOFA.ReturnOnFixedAssets.BaseYear;
            ReturnOnFixedAssets.CurrentYear = fAOFA.ReturnOnFixedAssets.CurrentYear;
        }
        private void RevolutionsFromReceivablesInit(IndicatorsOfTurnoverOfCurrentAssets iOTOCA)
        {
            RevolutionsFromReceivables.Number = "8.";
            RevolutionsFromReceivables.BaseYear = iOTOCA.RevolutionsFromReceivables.BaseYear;
            RevolutionsFromReceivables.CurrentYear = iOTOCA.RevolutionsFromReceivables.CurrentYear;
        }
        private void ReceivablesTurnoverInit(IndicatorsOfTurnoverOfCurrentAssets iOTOCA)
        {
            ReceivablesTurnover.Number = "9.";
            ReceivablesTurnover.BaseYear = iOTOCA.ReceivablesTurnover.BaseYear;
            ReceivablesTurnover.CurrentYear = iOTOCA.ReceivablesTurnover.CurrentYear;
        }
        private void TurnoverOfInventoriesRevolutionsInit(AfsModel model, CharacteristicsOfCapital cOC)
        {
            TurnoverOfInventoriesRevolutions.Number = "10.";
            TurnoverOfInventoriesRevolutions.BaseYear = model.F2Base.F2050.Current / ((cOC.TangibleCurrentAssets.Base.BeginningOfyear + cOC.TangibleCurrentAssets.Base.EndOfYear) / 2);
            TurnoverOfInventoriesRevolutions.CurrentYear = model.F2Current.F2050.Current / ((cOC.TangibleCurrentAssets.Current.BeginningOfyear + cOC.TangibleCurrentAssets.Current.EndOfYear) / 2);
        }
        private void TurnoverOfInventoriesDaysInit()
        {
            TurnoverOfInventoriesDays.Number = "11.";
            TurnoverOfInventoriesDays.BaseYear = AfsConstraints.DurationOAnalyzedPeriod / TurnoverOfInventoriesRevolutions.BaseYear;
            TurnoverOfInventoriesDays.CurrentYear = AfsConstraints.DurationOAnalyzedPeriod / TurnoverOfInventoriesRevolutions.CurrentYear;
        }
        private void OperatingCycleDurationInit()
        {
            OperatingCycleDuration.Number = "12.";
            OperatingCycleDuration.BaseYear = ReceivablesTurnover.BaseYear + TurnoverOfInventoriesDays.BaseYear;
            OperatingCycleDuration.CurrentYear = ReceivablesTurnover.CurrentYear + TurnoverOfInventoriesDays.CurrentYear;
        }
        private void NumberOfRevolutionsOfCurrentAssetsInit(IndicatorsOfTurnoverOfCurrentAssets iOTOCA)
        {
            NumberOfRevolutionsOfCurrentAssets.Number = "13.";
            NumberOfRevolutionsOfCurrentAssets.BaseYear = iOTOCA.NumberOfRevolutionsOfCurrentAssets.BaseYear;
            NumberOfRevolutionsOfCurrentAssets.CurrentYear = iOTOCA.NumberOfRevolutionsOfCurrentAssets.CurrentYear;
        }
        private void TurnoverOfWorkingCapitalInit(IndicatorsOfTurnoverOfCurrentAssets iOTOCA)
        {
            TurnoverOfWorkingCapital.Number = "14.";
            TurnoverOfWorkingCapital.BaseYear = iOTOCA.TurnoverOfWorkingCapital.BaseYear;
            TurnoverOfWorkingCapital.CurrentYear = iOTOCA.TurnoverOfWorkingCapital.CurrentYear;
        }
        private void TurnoverOfEquityInit(AfsModel model, SourcesOfCapitalFormation sOCF)
        {
            TurnoverOfEquity.Number = "15.";
            TurnoverOfEquity.BaseYear = model.F2Base.F2000.Current / ((sOCF.Equity.Base.BeginningOfyear + sOCF.Equity.Base.EndOfYear) / 2);
            TurnoverOfEquity.CurrentYear = model.F2Current.F2000.Current / ((sOCF.Equity.Current.BeginningOfyear + sOCF.Equity.Current.EndOfYear) / 2);
        }
        private void TurnoverOfTotalCapitalInit(AfsModel model, SourcesOfCapitalFormation sOCF)
        {
            TurnoverOfTotalCapital.Number = "16.";
            TurnoverOfTotalCapital.BaseYear = model.F2Base.F2000.Current / ((sOCF.TotalSourcesOfCapital.Base.BeginningOfyear + sOCF.TotalSourcesOfCapital.Base.EndOfYear) / 2);
            TurnoverOfTotalCapital.CurrentYear = model.F2Current.F2000.Current / ((sOCF.TotalSourcesOfCapital.Current.BeginningOfyear + sOCF.TotalSourcesOfCapital.Current.EndOfYear) / 2);
        }
        private void CoefficientOfStabilityOfEconomicGrowthInit(AfsModel model, SourcesOfCapitalFormation sOCF)
        {
            CoefficientOfStabilityOfEconomicGrowth.Number = "17.";
            CoefficientOfStabilityOfEconomicGrowth.BaseYear = model.F2Base.GetF2465Current() / ((sOCF.Equity.Base.BeginningOfyear + sOCF.Equity.Base.EndOfYear) / 2);
            CoefficientOfStabilityOfEconomicGrowth.CurrentYear = model.F2Current.GetF2465Current() / ((sOCF.Equity.Current.BeginningOfyear + sOCF.Equity.Current.EndOfYear) / 2);
        }
        private void ReturnOnEquityPeriodInit()
        {
            ReturnOnEquityPeriod.Number = "18.";
            ReturnOnEquityPeriod.BaseYear = 1 / CoefficientOfStabilityOfEconomicGrowth.BaseYear;
            ReturnOnEquityPeriod.CurrentYear = 1 / CoefficientOfStabilityOfEconomicGrowth.CurrentYear;
        }
    }
}
