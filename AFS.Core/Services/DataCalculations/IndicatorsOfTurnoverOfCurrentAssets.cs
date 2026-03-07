using AFS.Core.Models;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations
{
    /// <summary>
    /// Table 2: Indicators of Turnover of Current Assets
    /// Analyzes working capital efficiency and turnover rates
    /// Compares Base Year (2019) vs Current Year (2020)
    /// </summary>
    public class IndicatorsOfTurnoverOfCurrentAssets
    {
        /// <summary>
        /// 1. Average balances of current assets, thousand hryvnias
        /// Example: 50853351.0 (2019) → 39479820.0 (2020)
        /// </summary>
        public TwoYearsCalculationData AverageWorkingCapitalBalances { get; private set; } = new();

        /// <summary>
        /// 1.1. - Cash (sub-item of average balances)
        /// Example: 1051741.0 (2019) → 1200707.5 (2020)
        /// </summary>
        public TwoYearsCalculationData AverageFromMoney { get; private set; } = new();

        /// <summary>
        /// 1.2. - Accounts receivable (sub-item of average balances)
        /// Example: 41342595.0 (2019) → 32767614.0 (2020)
        /// </summary>
        public TwoYearsCalculationData AverageFromReceivables { get; private set; } = new();

        /// <summary>
        /// 1.3. - Material assets / Inventory (sub-item of average balances)
        /// Example: 8429844.0 (2019) → 5462601.5 (2020)
        /// </summary>
        public TwoYearsCalculationData AverageFromMaterialValues { get; private set; } = new();

        /// <summary>
        /// 2. Net revenue from sales, thousand hryvnias
        /// Example: 57293136.0 (2019) → 50563254.0 (2020)
        /// </summary>
        public TwoYearsCalculationData NetIncomeFromSales { get; private set; } = new();

        /// <summary>
        /// 2.1. One-day revenue from sales, thousand hryvnias
        /// Calculated as: Net revenue / 360 days
        /// Example: 159147.60 (2019) → 140453.48 (2020)
        /// </summary>
        public TwoYearsCalculationData OneDaySalesRevenue { get; private set; } = new();

        /// <summary>
        /// 3. Turnover of current assets, days
        /// Shows how many days it takes for working capital to complete one cycle
        /// Lower is better (faster turnover)
        /// Example: 319.54 (2019) → 281.09 (2020) = improvement of 38.45 days
        /// </summary>
        public TwoYearsCalculationData TurnoverOfWorkingCapital { get; private set; } = new();

        /// <summary>
        /// 3.1. - Turnover of cash, days
        /// Example: 6.61 (2019) → 8.55 (2020)
        /// </summary>
        public TwoYearsCalculationData MoneyTurnover { get; private set; } = new();

        /// <summary>
        /// 3.2. - Turnover of receivables, days
        /// Example: 259.78 (2019) → 233.30 (2020) = collection improved by 26.48 days
        /// </summary>
        public TwoYearsCalculationData ReceivablesTurnover { get; private set; } = new();

        /// <summary>
        /// 3.3. - Turnover of material assets, days
        /// Example: 52.97 (2019) → 38.89 (2020) = inventory turns faster by 14.08 days
        /// </summary>
        public TwoYearsCalculationData MaterialValuesTurnover { get; private set; } = new();

        /// <summary>
        /// 4. Number of turnovers of current assets, times
        /// How many times working capital cycles through in a year
        /// Higher is better (more frequent turnover)
        /// Example: 1.13 (2019) → 1.28 (2020) = 0.15 more cycles
        /// </summary>
        public TwoYearsCalculationData NumberOfRevolutionsOfCurrentAssets { get; private set; } = new();

        /// <summary>
        /// 4.1. - Cash turnovers, times
        /// Example: 54.47 (2019) → 42.11 (2020)
        /// </summary>
        public TwoYearsCalculationData RevolutionsFromMoney { get; private set; } = new();

        /// <summary>
        /// 4.2. - Accounts receivable turnovers, times
        /// Example: 1.39 (2019) → 1.54 (2020) = faster collection
        /// </summary>
        public TwoYearsCalculationData RevolutionsFromReceivables { get; private set; } = new();

        /// <summary>
        /// 4.3. - Material assets turnovers, times
        /// Example: 6.80 (2019) → 9.26 (2020) = inventory turns 2.46 more times
        /// </summary>
        public TwoYearsCalculationData RevolutionsFromTangibleAssets { get; private set; } = new();

        /// <summary>
        /// 5. Working capital turnover ratio
        /// Lower is better (less capital tied up per unit of revenue)
        /// Example: 0.8876 (2019) → 0.7808 (2020) = efficiency improved
        /// Note: Growth rate shown as "x" (not applicable for ratios)
        /// </summary>
        public TwoYearsCalculationData FixingRatioOfCurrentAssets { get; private set; } = new();

        /// <summary>
        /// 6. Release (-), absence of current assets (+) due to changes in their turnover
        /// Negative value = capital RELEASED (freed up) - GOOD
        /// Positive value = MORE capital NEEDED - CONCERNING
        /// Example: -5400092.0258 (2020) = capital freed up by improved efficiency
        /// Only calculated for current year (comparison to base year)
        /// </summary>
        public TwoYearsCalculationData ReleaseOrLackOfCurrentAssetsDueTurnover { get; private set; } = new();

        public IndicatorsOfTurnoverOfCurrentAssets(AfsModel model) => Init(model);

        private void Init(AfsModel model)
        {
            // Row 1: Average balances
            IndicatorsOfTurnoverOfCurrentAssetsInit(model);
            AverageFromMoneyInit(model);
            AverageFromReceivablesInit(model);
            AverageFromTangibleAssetsInit(model);

            // Row 2: Revenue
            NetIncomeFromSalesInit(model);
            OneDaySalesRevenueInit();

            // Row 3: Turnover in days
            TurnoverOfWorkingCapitalInit();
            MoneyTurnoverInit();
            ReceivablesTurnoverInit();
            MaterialValuesTurnoverInit();

            // Row 4: Number of turnovers
            NumberOfRevolutionsOfCurrentAssetsInit();
            RevolutionsFromMoneyInit();
            RevolutionsFromReceivablesInit();
            RevolutionsFromTangibleAssetsInit();

            // Row 5-6: Ratios and capital release
            FixingRatioOfCurrentAssetsInit();
            ReleaseOrLackOfCurrentAssetsDueTurnoverInit();
        }

        /// <summary>
        /// 1. Average balances of current assets, thousand hryvnias
        /// Formula: (Beginning balance + Ending balance) / 2
        /// Includes: Current assets (Line 1195) - Future expenses (Line 1170) + Assets held for sale (Line 1200)
        /// </summary>
        private void IndicatorsOfTurnoverOfCurrentAssetsInit(AfsModel model)
        {
            AverageWorkingCapitalBalances.Number = "1.";

            // Base Year (2019): Average = (Beginning + Ending) / 2
            // 50853351.0 = (59994694 + 41712008) / 2
            AverageWorkingCapitalBalances.BaseYear = (
                model.F1Base.GetF1195Begin() +
                model.F1Base.GetF1195End() -
                model.F1Base.F1170.Begin -
                model.F1Base.F1170.End +
                model.F1Base.F1200.Begin +
                model.F1Base.F1200.End
            ) / 2;

            // Current Year (2020): Average = (Beginning + Ending) / 2
            // 39479820.0 = (41712008 + 37247632) / 2
            AverageWorkingCapitalBalances.CurrentYear = (
                model.F1Current.GetF1195Begin() +
                model.F1Current.GetF1195End() -
                model.F1Current.F1170.Begin -
                model.F1Current.F1170.End +
                model.F1Current.F1200.Begin +
                model.F1Current.F1200.End
            ) / 2;
        }

        /// <summary>
        /// 1.1. - Cash (average balances)
        /// Formula: (Beginning cash + Ending cash) / 2
        /// Includes: Cash (Line 1160) + Current financial investments (Line 1165)
        /// </summary>
        private void AverageFromMoneyInit(AfsModel model)
        {
            AverageFromMoney.Number = "1.1.";

            // Base Year (2019): 1051741.0
            AverageFromMoney.BaseYear = (
                model.F1Base.GetAccountsMoney(true) +
                model.F1Base.GetAccountsMoney(false)
            ) / 2;

            // Current Year (2020): 1200707.5
            AverageFromMoney.CurrentYear = (
                model.F1Current.F1160.Begin +
                model.F1Current.F1165.Begin +
                model.F1Current.F1160.End +
                model.F1Current.F1165.End
            ) / 2;
        }

        /// <summary>
        /// 1.2. - Accounts receivable (average balances)
        /// Formula: (Beginning AR + Ending AR) / 2
        /// </summary>
        private void AverageFromReceivablesInit(AfsModel model)
        {
            AverageFromReceivables.Number = "1.2.";

            // Base Year (2019): 41342595.0
            AverageFromReceivables.BaseYear = (
                model.F1Base.GetAccountsReceivable(true) +
                model.F1Base.GetAccountsReceivable(false)
            ) / 2;

            // Current Year (2020): 32767614.0
            AverageFromReceivables.CurrentYear = (
                model.F1Current.GetAccountsReceivable(true) +
                model.F1Current.GetAccountsReceivable(false)
            ) / 2;
        }

        /// <summary>
        /// 1.3. - Material assets / Inventory (average balances)
        /// Formula: (Beginning inventory + Ending inventory) / 2
        /// Includes: Inventory (Line 1100) + Current biological assets (Line 1110)
        /// </summary>
        private void AverageFromTangibleAssetsInit(AfsModel model)
        {
            AverageFromMaterialValues.Number = "1.3.";

            // Base Year (2019): 8429844.0
            AverageFromMaterialValues.BaseYear = (
                model.F1Base.GetAccountsTangibleAssets(true) +
                model.F1Base.GetAccountsTangibleAssets(false)
            ) / 2;

            // Current Year (2020): 5462601.5
            AverageFromMaterialValues.CurrentYear = (
                model.F1Current.GetF1100Begin() +
                model.F1Current.F1110.Begin +
                model.F1Current.GetF1100End() +
                model.F1Current.F1110.End
            ) / 2;
        }

        /// <summary>
        /// 2. Net revenue from sales, thousand hryvnias
        /// Taken directly from Income Statement (Form 2), Line 2000
        /// </summary>
        private void NetIncomeFromSalesInit(AfsModel model)
        {
            NetIncomeFromSales.Number = "2.";

            // Base Year (2019): 57293136.0
            NetIncomeFromSales.BaseYear = model.F2Base.F2000.Current;

            // Current Year (2020): 50563254.0
            NetIncomeFromSales.CurrentYear = model.F2Current.F2000.Current;
        }

        /// <summary>
        /// 2.1. One-day revenue from sales, thousand hryvnias
        /// Formula: Net revenue / 360 days
        /// Used as baseline for calculating turnover periods in days
        /// </summary>
        private void OneDaySalesRevenueInit()
        {
            OneDaySalesRevenue.Number = "2.1.";

            // Base Year (2019): 57293136.0 / 360 = 159147.60
            OneDaySalesRevenue.BaseYear = NetIncomeFromSales.BaseYear / AfsConstraints.DurationOAnalyzedPeriod;

            // Current Year (2020): 50563254.0 / 360 = 140453.48
            OneDaySalesRevenue.CurrentYear = NetIncomeFromSales.CurrentYear / AfsConstraints.DurationOAnalyzedPeriod;
        }

        /// <summary>
        /// 3. Turnover of current assets, days
        /// Formula: Average working capital / One-day revenue
        /// Shows operating cycle length - lower is better
        /// </summary>
        private void TurnoverOfWorkingCapitalInit()
        {
            TurnoverOfWorkingCapital.Number = "3.";

            // Base Year (2019): 50853351.0 / 159147.60 = 319.54 days
            TurnoverOfWorkingCapital.BaseYear = SafeDivide(
                AverageWorkingCapitalBalances.BaseYear,
                OneDaySalesRevenue.BaseYear
            );

            // Current Year (2020): 39479820.0 / 140453.48 = 281.09 days
            // Improvement of 38.45 days (faster cycle)
            TurnoverOfWorkingCapital.CurrentYear = SafeDivide(
                AverageWorkingCapitalBalances.CurrentYear,
                OneDaySalesRevenue.CurrentYear
            );
        }

        /// <summary>
        /// 3.1. - Turnover of cash, days
        /// Formula: Average cash balance / One-day revenue
        /// </summary>
        private void MoneyTurnoverInit()
        {
            MoneyTurnover.Number = "3.1.";

            // Base Year (2019): 1051741.0 / 159147.60 = 6.61 days
            MoneyTurnover.BaseYear = SafeDivide(
                AverageFromMoney.BaseYear,
                OneDaySalesRevenue.BaseYear
            );

            // Current Year (2020): 1200707.5 / 140453.48 = 8.55 days
            MoneyTurnover.CurrentYear = SafeDivide(
                AverageFromMoney.CurrentYear,
                OneDaySalesRevenue.CurrentYear
            );
        }

        /// <summary>
        /// 3.2. - Turnover of receivables, days (Collection period)
        /// Formula: Average receivables / One-day revenue
        /// Lower is better (faster collection)
        /// </summary>
        private void ReceivablesTurnoverInit()
        {
            ReceivablesTurnover.Number = "3.2.";

            // Base Year (2019): 41342595.0 / 159147.60 = 259.78 days
            ReceivablesTurnover.BaseYear = SafeDivide(
                AverageFromReceivables.BaseYear,
                OneDaySalesRevenue.BaseYear
            );

            // Current Year (2020): 32767614.0 / 140453.48 = 233.30 days
            // Improved collection by 26.48 days
            ReceivablesTurnover.CurrentYear = SafeDivide(
                AverageFromReceivables.CurrentYear,
                OneDaySalesRevenue.CurrentYear
            );
        }

        /// <summary>
        /// 3.3. - Turnover of material assets, days (Inventory holding period)
        /// Formula: Average inventory / One-day revenue
        /// Lower is better (faster inventory turnover)
        /// </summary>
        private void MaterialValuesTurnoverInit()
        {
            MaterialValuesTurnover.Number = "3.3.";

            // Base Year (2019): 8429844.0 / 159147.60 = 52.97 days
            MaterialValuesTurnover.BaseYear = SafeDivide(
                AverageFromMaterialValues.BaseYear,
                OneDaySalesRevenue.BaseYear
            );

            // Current Year (2020): 5462601.5 / 140453.48 = 38.89 days
            // Inventory turns 14.08 days faster
            MaterialValuesTurnover.CurrentYear = SafeDivide(
                AverageFromMaterialValues.CurrentYear,
                OneDaySalesRevenue.CurrentYear
            );
        }

        /// <summary>
        /// 4. Number of turnovers of current assets, times
        /// Formula: Net revenue / Average working capital
        /// Higher is better (capital cycles more frequently)
        /// </summary>
        private void NumberOfRevolutionsOfCurrentAssetsInit()
        {
            NumberOfRevolutionsOfCurrentAssets.Number = "4.";

            // Base Year (2019): 57293136.0 / 50853351.0 = 1.13 times
            NumberOfRevolutionsOfCurrentAssets.BaseYear = SafeDivide(
                NetIncomeFromSales.BaseYear,
                AverageWorkingCapitalBalances.BaseYear
            );

            // Current Year (2020): 50563254.0 / 39479820.0 = 1.28 times
            // Working capital cycles 0.15 times more frequently
            NumberOfRevolutionsOfCurrentAssets.CurrentYear = SafeDivide(
                NetIncomeFromSales.CurrentYear,
                AverageWorkingCapitalBalances.CurrentYear
            );
        }

        /// <summary>
        /// 4.1. - Cash turnovers, times
        /// Formula: Net revenue / Average cash balance
        /// </summary>
        private void RevolutionsFromMoneyInit()
        {
            RevolutionsFromMoney.Number = "4.1.";

            // Base Year (2019): 57293136.0 / 1051741.0 = 54.47 times
            RevolutionsFromMoney.BaseYear = SafeDivide(
                NetIncomeFromSales.BaseYear,
                AverageFromMoney.BaseYear
            );

            // Current Year (2020): 50563254.0 / 1200707.5 = 42.11 times
            RevolutionsFromMoney.CurrentYear = SafeDivide(
                NetIncomeFromSales.CurrentYear,
                AverageFromMoney.CurrentYear
            );
        }

        /// <summary>
        /// 4.2. - Accounts receivable turnovers, times
        /// Formula: Net revenue / Average receivables
        /// Higher is better (faster collection)
        /// </summary>
        private void RevolutionsFromReceivablesInit()
        {
            RevolutionsFromReceivables.Number = "4.2.";

            // Base Year (2019): 57293136.0 / 41342595.0 = 1.39 times
            RevolutionsFromReceivables.BaseYear = SafeDivide(
                NetIncomeFromSales.BaseYear,
                AverageFromReceivables.BaseYear
            );

            // Current Year (2020): 50563254.0 / 32767614.0 = 1.54 times
            // AR turns 0.16 times more frequently
            RevolutionsFromReceivables.CurrentYear = SafeDivide(
                NetIncomeFromSales.CurrentYear,
                AverageFromReceivables.CurrentYear
            );
        }

        /// <summary>
        /// 4.3. - Material assets turnovers, times (Inventory turnover ratio)
        /// Formula: Net revenue / Average inventory
        /// Higher is better (inventory moves faster)
        /// </summary>
        private void RevolutionsFromTangibleAssetsInit()
        {
            RevolutionsFromTangibleAssets.Number = "4.3.";

            // Base Year (2019): 57293136.0 / 8429844.0 = 6.80 times
            RevolutionsFromTangibleAssets.BaseYear = SafeDivide(
                NetIncomeFromSales.BaseYear,
                AverageFromMaterialValues.BaseYear
            );

            // Current Year (2020): 50563254.0 / 5462601.5 = 9.26 times
            // Inventory turns 2.46 times more frequently (significant improvement)
            RevolutionsFromTangibleAssets.CurrentYear = SafeDivide(
                NetIncomeFromSales.CurrentYear,
                AverageFromMaterialValues.CurrentYear
            );
        }

        /// <summary>
        /// 5. Working capital turnover ratio
        /// Formula: Average working capital / Net revenue
        /// Lower is better (less capital needed per unit of revenue)
        /// Inverse of row 4 (Number of turnovers)
        /// </summary>
        private void FixingRatioOfCurrentAssetsInit()
        {
            FixingRatioOfCurrentAssets.Number = "5.";

            // Base Year (2019): 50853351.0 / 57293136.0 = 0.8876
            FixingRatioOfCurrentAssets.BaseYear = SafeDivide(
                AverageWorkingCapitalBalances.BaseYear,
                NetIncomeFromSales.BaseYear
            );

            // Current Year (2020): 39479820.0 / 50563254.0 = 0.7808
            // Improvement of 0.1068 (more efficient use of capital)
            FixingRatioOfCurrentAssets.CurrentYear = SafeDivide(
                AverageWorkingCapitalBalances.CurrentYear,
                NetIncomeFromSales.CurrentYear
            );
        }

        /// <summary>
        /// 6. Release (-), absence of current assets (+) due to changes in their turnover
        /// Formula: One-day revenue (current year) × Change in turnover days
        /// Negative = Capital RELEASED (freed up) - GOOD
        /// Positive = MORE capital NEEDED - BAD
        /// Only calculated for current year (shows impact of efficiency change)
        /// </summary>
        private void ReleaseOrLackOfCurrentAssetsDueTurnoverInit()
        {
            ReleaseOrLackOfCurrentAssetsDueTurnover.Number = "6.";

            // Current Year (2020) only:
            // 140453.48 × (281.09 - 319.54) = 140453.48 × (-38.45) = -5400092.0258
            // Negative value means 5.4 million UAH of capital was FREED UP
            // This happened because turnover improved by 38.45 days
            ReleaseOrLackOfCurrentAssetsDueTurnover.CurrentYear =
                OneDaySalesRevenue.CurrentYear * TurnoverOfWorkingCapital.Deviations;
        }

        /// <summary>
        /// Safe division to prevent division by zero errors
        /// Returns 0 if denominator is zero or result is NaN/Infinity
        /// </summary>
        private static double SafeDivide(double numerator, double denominator)
        {
            if (AfsConstraints.IsZeroOrInvalid(denominator))
                return 0;

            var result = numerator / denominator;

            if (double.IsNaN(result) || double.IsInfinity(result))
                return 0;

            return result;
        }
    }
}
