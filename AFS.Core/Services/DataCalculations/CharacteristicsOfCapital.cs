using AFS.Core.Models;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations
{
    public class CharacteristicsOfCapital
    {
        public TotalAssets TotalAssets { get; private set; } = new();
        public NonCurrentImmobilizedFunds NonCurrentImmobilizedFunds { get; private set; } = new();
        public CurrentMobileAssets CurrentMobileAssets { get; private set; } = new();
        public TangibleCurrentAssets TangibleCurrentAssets { get; private set; } = new();
        public AccountsReceivable AccountsReceivable { get; private set; } = new();
        public CashCurrentFinancialInvestments CashCurrentFinancialInvestments { get; private set; } = new();
        public OtherCurrentAssets OtherCurrentAssets { get; private set; } = new();
        public NonCurrentAssetsHeldForSale NonCurrentAssetsHeldForSale { get; private set; } = new();
        public FutureExpenses FutureExpenses { get; private set; } = new();

        public CharacteristicsOfCapital(AFSModel model) => Init(model);

        private void Init(AFSModel model)
        {
            TotalAssets.Init(model);
            NonCurrentImmobilizedFunds.Init(model, TotalAssets);
            CurrentMobileAssets.Init(model, TotalAssets);
            TangibleCurrentAssets.Init(model, CurrentMobileAssets);
            AccountsReceivable.Init(model, CurrentMobileAssets);
            CashCurrentFinancialInvestments.Init(model, CurrentMobileAssets);
            OtherCurrentAssets.Init(model, CurrentMobileAssets);
            NonCurrentAssetsHeldForSale.Init(model, CurrentMobileAssets);
            FutureExpenses.Init(model, TotalAssets);
        }
    }

    /// <summary>
    /// Total capital (assets)
    /// Row header in the table with no numbering
    /// </summary>
    public class TotalAssets
    {
        public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();

        public void Init(AFSModel model)
        {
            // Base Year (2019)
            Base.BeginningOfyear = model.F1Base.GetF1300Begin();
            Base.EndOfYear = model.F1Base.GetF1300End();

            // Current Year (2020)
            Current.BeginningOfyear = model.F1Current.GetF1300Begin();
            Current.EndOfYear = model.F1Current.GetF1300End();
        }
    }

    /// <summary>
    /// 1. Non-current (fixed) assets
    /// Also shown as % of assets
    /// </summary>
    public class NonCurrentImmobilizedFunds
    {
        public string Number { get; private set; } = "1.";

        /// <summary>
        /// Non-current assets in absolute values for base year
        /// </summary>
        public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();

        /// <summary>
        /// Non-current assets in absolute values for current year
        /// </summary>
        public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();

        /// <summary>
        /// Non-current assets as % of total assets for base year
        /// </summary>
        public CharacteristicsOfCapitalCalculationRow InPercentageOfAssetsBase { get; private set; } = new();

        /// <summary>
        /// Non-current assets as % of total assets for current year
        /// </summary>
        public CharacteristicsOfCapitalCalculationRow InPercentageOfAssetsCurrent { get; private set; } = new();

        public void Init(AFSModel model, TotalAssets totalAssets)
        {
            // Base Year (2019) - Absolute values
            // Form 1, Line 1095: Non-current assets
            Base.BeginningOfyear = model.F1Base.GetF1095Begin();
            Base.EndOfYear = model.F1Base.GetF1095End();

            // Current Year (2020) - Absolute values
            Current.BeginningOfyear = model.F1Current.GetF1095Begin();
            Current.EndOfYear = model.F1Current.GetF1095End();

            // Base Year (2019) - As % of total assets
            // Example: 30800401 / 91647626 * 100 = 33.6%
            InPercentageOfAssetsBase.BeginningOfyear = SafeDivide(Base.BeginningOfyear, totalAssets.Base.BeginningOfyear) * 100;
            InPercentageOfAssetsBase.EndOfYear = SafeDivide(Base.EndOfYear, totalAssets.Base.EndOfYear) * 100;

            // Current Year (2020) - As % of total assets
            // Example: 34631296 / 77599288 * 100 = 44.6%
            InPercentageOfAssetsCurrent.BeginningOfyear = SafeDivide(Current.BeginningOfyear, totalAssets.Current.BeginningOfyear) * 100;
            InPercentageOfAssetsCurrent.EndOfYear = SafeDivide(Current.EndOfYear, totalAssets.Current.EndOfYear) * 100;
        }

        private static double SafeDivide(double numerator, double denominator)
        {
            if (denominator == 0 || double.IsNaN(denominator) || double.IsInfinity(denominator))
                return 0;

            var result = numerator / denominator;

            if (double.IsNaN(result) || double.IsInfinity(result))
                return 0;

            return result;
        }
    }

    /// <summary>
    /// 2. Current (mobile) assets
    /// Also shown as % of assets
    /// </summary>
    public class CurrentMobileAssets
    {
        public string Number { get; private set; } = "2.";

        /// <summary>
        /// Current assets in absolute values for base year
        /// </summary>
        public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();

        /// <summary>
        /// Current assets in absolute values for current year
        /// </summary>
        public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();

        /// <summary>
        /// Current assets as % of total assets for base year
        /// </summary>
        public CharacteristicsOfCapitalCalculationRow InPercentageOfAssetsBase { get; private set; } = new();

        /// <summary>
        /// Current assets as % of total assets for current year
        /// </summary>
        public CharacteristicsOfCapitalCalculationRow InPercentageOfAssetsCurrent { get; private set; } = new();

        public void Init(AFSModel model, TotalAssets totalAssets)
        {
            // Base Year (2019) - Absolute values
            // Form 1, Line 1195 (Current assets) - Line 1170 (Future expenses) + Line 1200 (Non-current assets held for sale)
            Base.BeginningOfyear = model.F1Base.GetF1195Begin() - model.F1Base.F1170.Begin + model.F1Base.F1200.Begin;
            Base.EndOfYear = model.F1Base.GetF1195End() - model.F1Base.F1170.End + model.F1Base.F1200.End;

            // Current Year (2020) - Absolute values
            Current.BeginningOfyear = model.F1Current.GetF1195Begin() - model.F1Current.F1170.Begin + model.F1Current.F1200.Begin;
            Current.EndOfYear = model.F1Current.GetF1195End() - model.F1Current.F1170.End + model.F1Current.F1200.End;

            // Base Year (2019) - As % of total assets
            // Example: 59994694 / 91647626 * 100 = 65.5%
            InPercentageOfAssetsBase.BeginningOfyear = SafeDivide(Base.BeginningOfyear, totalAssets.Base.BeginningOfyear) * 100;
            InPercentageOfAssetsBase.EndOfYear = SafeDivide(Base.EndOfYear, totalAssets.Base.EndOfYear) * 100;

            // Current Year (2020) - As % of total assets
            // Example: 41712008 / 77599288 * 100 = 53.8%
            InPercentageOfAssetsCurrent.BeginningOfyear = SafeDivide(Current.BeginningOfyear, totalAssets.Current.BeginningOfyear) * 100;
            InPercentageOfAssetsCurrent.EndOfYear = SafeDivide(Current.EndOfYear, totalAssets.Current.EndOfYear) * 100;
        }

        private static double SafeDivide(double numerator, double denominator)
        {
            if (denominator == 0 || double.IsNaN(denominator) || double.IsInfinity(denominator))
                return 0;

            var result = numerator / denominator;

            if (double.IsNaN(result) || double.IsInfinity(result))
                return 0;

            return result;
        }
    }

    /// <summary>
    /// 2.1. Material current assets (Inventory)
    /// Shown as a percentage of working capital
    /// </summary>
    public class TangibleCurrentAssets
    {
        public string Number { get; private set; } = "2.1.";
        public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow InPercentageOfCurrentAssetsBase { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow InPercentageOfCurrentAssetsCurrent { get; private set; } = new();

        public void Init(AFSModel model, CurrentMobileAssets currentMobileAssets)
        {
            // Base Year (2019) - Absolute values
            Base.BeginningOfyear = model.F1Base.GetAccountsTangibleAssets(true);
            Base.EndOfYear = model.F1Base.GetAccountsTangibleAssets(false);

            // Current Year (2020) - Absolute values
            Current.BeginningOfyear = model.F1Current.GetAccountsTangibleAssets(true);
            Current.EndOfYear = model.F1Current.GetAccountsTangibleAssets(false);

            // Base Year (2019) - As % of working capital
            // Example: 11041670 / 59994694 * 100 = 18.4%
            InPercentageOfCurrentAssetsBase.BeginningOfyear = SafeDivide(Base.BeginningOfyear, currentMobileAssets.Base.BeginningOfyear) * 100;
            InPercentageOfCurrentAssetsBase.EndOfYear = SafeDivide(Base.EndOfYear, currentMobileAssets.Base.EndOfYear) * 100;

            // Current Year (2020) - As % of working capital
            // Example: 5818018 / 41712008 * 100 = 13.9%
            InPercentageOfCurrentAssetsCurrent.BeginningOfyear = SafeDivide(Current.BeginningOfyear, currentMobileAssets.Current.BeginningOfyear) * 100;
            InPercentageOfCurrentAssetsCurrent.EndOfYear = SafeDivide(Current.EndOfYear, currentMobileAssets.Current.EndOfYear) * 100;
        }

        private static double SafeDivide(double numerator, double denominator)
        {
            if (denominator == 0 || double.IsNaN(denominator) || double.IsInfinity(denominator))
                return 0;

            var result = numerator / denominator;

            if (double.IsNaN(result) || double.IsInfinity(result))
                return 0;

            return result;
        }
    }

    /// <summary>
    /// 2.2. Accounts receivable
    /// Shown as a percentage of working capital
    /// </summary>
    public class AccountsReceivable
    {
        public string Number { get; private set; } = "2.2.";
        public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow InPercentageOfCurrentAssetsBase { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow InPercentageOfCurrentAssetsCurrent { get; private set; } = new();

        public void Init(AFSModel model, CurrentMobileAssets currentMobileAssets)
        {
            // Base Year (2019) - Absolute values
            Base.BeginningOfyear = model.F1Base.GetAccountsReceivable(true);
            Base.EndOfYear = model.F1Base.GetAccountsReceivable(false);

            // Current Year (2020) - Absolute values
            Current.BeginningOfyear = model.F1Current.GetAccountsReceivable(true);
            Current.EndOfYear = model.F1Current.GetAccountsReceivable(false);

            // Base Year (2019) - As % of working capital
            // Example: 47595592 / 59994694 * 100 = 79.3%
            InPercentageOfCurrentAssetsBase.BeginningOfyear = SafeDivide(Base.BeginningOfyear, currentMobileAssets.Base.BeginningOfyear) * 100;
            InPercentageOfCurrentAssetsBase.EndOfYear = SafeDivide(Base.EndOfYear, currentMobileAssets.Base.EndOfYear) * 100;

            // Current Year (2020) - As % of working capital
            // Example: 35089598 / 41712008 * 100 = 84.1%
            InPercentageOfCurrentAssetsCurrent.BeginningOfyear = SafeDivide(Current.BeginningOfyear, currentMobileAssets.Current.BeginningOfyear) * 100;
            InPercentageOfCurrentAssetsCurrent.EndOfYear = SafeDivide(Current.EndOfYear, currentMobileAssets.Current.EndOfYear) * 100;
        }

        private static double SafeDivide(double numerator, double denominator)
        {
            if (denominator == 0 || double.IsNaN(denominator) || double.IsInfinity(denominator))
                return 0;

            var result = numerator / denominator;

            if (double.IsNaN(result) || double.IsInfinity(result))
                return 0;

            return result;
        }
    }

    /// <summary>
    /// 2.3. Cash and current financial investments
    /// Shown as a percentage of working capital
    /// </summary>
    public class CashCurrentFinancialInvestments
    {
        public string Number { get; private set; } = "2.3.";
        public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow InPercentageOfCurrentAssetsBase { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow InPercentageOfCurrentAssetsCurrent { get; private set; } = new();

        public void Init(AFSModel model, CurrentMobileAssets currentMobileAssets)
        {
            // Base Year (2019) - Absolute values
            Base.BeginningOfyear = model.F1Base.GetAccountsMoney(true);
            Base.EndOfYear = model.F1Base.GetAccountsMoney(false);

            // Current Year (2020) - Absolute values
            Current.BeginningOfyear = model.F1Current.GetAccountsMoney(true);
            Current.EndOfYear = model.F1Current.GetAccountsMoney(false);

            // Base Year (2019) - As % of working capital
            // Example: 1299090 / 59994694 * 100 = 2.2%
            InPercentageOfCurrentAssetsBase.BeginningOfyear = SafeDivide(Base.BeginningOfyear, currentMobileAssets.Base.BeginningOfyear) * 100;
            InPercentageOfCurrentAssetsBase.EndOfYear = SafeDivide(Base.EndOfYear, currentMobileAssets.Base.EndOfYear) * 100;

            // Current Year (2020) - As % of working capital
            // Example: 804392 / 41712008 * 100 = 1.9%
            InPercentageOfCurrentAssetsCurrent.BeginningOfyear = SafeDivide(Current.BeginningOfyear, currentMobileAssets.Current.BeginningOfyear) * 100;
            InPercentageOfCurrentAssetsCurrent.EndOfYear = SafeDivide(Current.EndOfYear, currentMobileAssets.Current.EndOfYear) * 100;
        }

        private static double SafeDivide(double numerator, double denominator)
        {
            if (denominator == 0 || double.IsNaN(denominator) || double.IsInfinity(denominator))
                return 0;

            var result = numerator / denominator;

            if (double.IsNaN(result) || double.IsInfinity(result))
                return 0;

            return result;
        }
    }

    /// <summary>
    /// 2.4. Other current assets
    /// Shown as a percentage of working capital
    /// </summary>
    public class OtherCurrentAssets
    {
        public string Number { get; private set; } = "2.4.";
        public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow InPercentageOfCurrentAssetsBase { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow InPercentageOfCurrentAssetsCurrent { get; private set; } = new();

        public void Init(AFSModel model, CurrentMobileAssets currentMobileAssets)
        {
            // Base Year (2019) - Absolute values
            // Form 1, Line 1190: Other current assets
            Base.BeginningOfyear = model.F1Base.F1190.Begin;
            Base.EndOfYear = model.F1Base.F1190.End;

            // Current Year (2020) - Absolute values
            Current.BeginningOfyear = model.F1Current.F1190.Begin;
            Current.EndOfYear = model.F1Current.F1190.End;

            // Base Year (2019) - As % of working capital
            // Example: 58342 / 59994694 * 100 = 0.1%
            InPercentageOfCurrentAssetsBase.BeginningOfyear = SafeDivide(Base.BeginningOfyear, currentMobileAssets.Base.BeginningOfyear) * 100;
            InPercentageOfCurrentAssetsBase.EndOfYear = SafeDivide(Base.EndOfYear, currentMobileAssets.Base.EndOfYear) * 100;

            // Current Year (2020) - As % of working capital
            // Example: 0 / 41712008 * 100 = 0.0% (beginning), 97794 / 37247632 * 100 = 0.3% (end)
            InPercentageOfCurrentAssetsCurrent.BeginningOfyear = SafeDivide(Current.BeginningOfyear, currentMobileAssets.Current.BeginningOfyear) * 100;
            InPercentageOfCurrentAssetsCurrent.EndOfYear = SafeDivide(Current.EndOfYear, currentMobileAssets.Current.EndOfYear) * 100;
        }

        private static double SafeDivide(double numerator, double denominator)
        {
            if (denominator == 0 || double.IsNaN(denominator) || double.IsInfinity(denominator))
                return 0;

            var result = numerator / denominator;

            if (double.IsNaN(result) || double.IsInfinity(result))
                return 0;

            return result;
        }
    }

    /// <summary>
    /// 2.5. Non-current assets held for sale and disposal groups
    /// Shown as a percentage of working capital
    /// </summary>
    public class NonCurrentAssetsHeldForSale
    {
        public string Number { get; private set; } = "2.5";
        public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow InPercentageOfCurrentAssetsBase { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow InPercentageOfCurrentAssetsCurrent { get; private set; } = new();

        public void Init(AFSModel model, CurrentMobileAssets currentMobileAssets)
        {
            // Base Year (2019) - Absolute values
            // Form 1, Line 1200: Non-current assets held for sale
            Base.BeginningOfyear = model.F1Base.F1200.Begin;
            Base.EndOfYear = model.F1Base.F1200.End;

            // Current Year (2020) - Absolute values
            Current.BeginningOfyear = model.F1Current.F1200.Begin;
            Current.EndOfYear = model.F1Current.F1200.End;

            // Base Year (2019) - As % of working capital
            InPercentageOfCurrentAssetsBase.BeginningOfyear = SafeDivide(Base.BeginningOfyear, currentMobileAssets.Base.BeginningOfyear) * 100;
            InPercentageOfCurrentAssetsBase.EndOfYear = SafeDivide(Base.EndOfYear, currentMobileAssets.Base.EndOfYear) * 100;

            // Current Year (2020) - As % of working capital
            InPercentageOfCurrentAssetsCurrent.BeginningOfyear = SafeDivide(Current.BeginningOfyear, currentMobileAssets.Current.BeginningOfyear) * 100;
            InPercentageOfCurrentAssetsCurrent.EndOfYear = SafeDivide(Current.EndOfYear, currentMobileAssets.Current.EndOfYear) * 100;
        }

        private static double SafeDivide(double numerator, double denominator)
        {
            if (denominator == 0 || double.IsNaN(denominator) || double.IsInfinity(denominator))
                return 0;

            var result = numerator / denominator;

            if (double.IsNaN(result) || double.IsInfinity(result))
                return 0;

            return result;
        }
    }

    /// <summary>
    /// 3. Future period expenses
    /// Also shown as % of assets
    /// </summary>
    public class FutureExpenses
    {
        public string Number { get; private set; } = "3.";
        public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow InPercentageOfAssetsBase { get; private set; } = new();
        public CharacteristicsOfCapitalCalculationRow InPercentageOfAssetsCurrent { get; private set; } = new();

        public void Init(AFSModel model, TotalAssets totalAssets)
        {
            // Base Year (2019) - Absolute values
            // Form 1, Line 1170: Future expenses
            Base.BeginningOfyear = model.F1Base.F1170.Begin;
            Base.EndOfYear = model.F1Base.F1170.End;

            // Current Year (2020) - Absolute values
            Current.BeginningOfyear = model.F1Current.F1170.Begin;
            Current.EndOfYear = model.F1Current.F1170.End;

            // Base Year (2019) - As % of total assets
            // Example: 852531 / 91647626 * 100 = 0.9%
            InPercentageOfAssetsBase.BeginningOfyear = SafeDivide(Base.BeginningOfyear, totalAssets.Base.BeginningOfyear) * 100;
            InPercentageOfAssetsBase.EndOfYear = SafeDivide(Base.EndOfYear, totalAssets.Base.EndOfYear) * 100;

            // Current Year (2020) - As % of total assets
            // Example: 1255984 / 77599288 * 100 = 1.6%
            InPercentageOfAssetsCurrent.BeginningOfyear = SafeDivide(Current.BeginningOfyear, totalAssets.Current.BeginningOfyear) * 100;
            InPercentageOfAssetsCurrent.EndOfYear = SafeDivide(Current.EndOfYear, totalAssets.Current.EndOfYear) * 100;
        }

        private static double SafeDivide(double numerator, double denominator)
        {
            if (denominator == 0 || double.IsNaN(denominator) || double.IsInfinity(denominator))
                return 0;

            var result = numerator / denominator;

            if (double.IsNaN(result) || double.IsInfinity(result))
                return 0;

            return result;
        }
    }
}