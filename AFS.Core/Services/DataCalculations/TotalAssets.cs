using AFS.Core.Models;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

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
    public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow InPercentageOfAssetsBase { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow InPercentageOfAssetsCurrent { get; private set; } = new();

    public void Init(AFSModel model, TotalAssets totalAssets)
    {
        Base.BeginningOfyear = model.F1Base.GetF1095Begin();
        Base.EndOfYear = model.F1Base.GetF1095End();
        Current.BeginningOfyear = model.F1Current.GetF1095Begin();
        Current.EndOfYear = model.F1Current.GetF1095End();

        InPercentageOfAssetsBase.BeginningOfyear = AFSConstraints.SafeDivide(Base.BeginningOfyear, totalAssets.Base.BeginningOfyear) * 100;
        InPercentageOfAssetsBase.EndOfYear = AFSConstraints.SafeDivide(Base.EndOfYear, totalAssets.Base.EndOfYear) * 100;
        InPercentageOfAssetsCurrent.BeginningOfyear = AFSConstraints.SafeDivide(Current.BeginningOfyear, totalAssets.Current.BeginningOfyear) * 100;
        InPercentageOfAssetsCurrent.EndOfYear = AFSConstraints.SafeDivide(Current.EndOfYear, totalAssets.Current.EndOfYear) * 100;
    }
}

/// <summary>
/// 2. Current (mobile) assets
/// </summary>
public class CurrentMobileAssets
{
    public string Number { get; private set; } = "2.";
    public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow InPercentageOfAssetsBase { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow InPercentageOfAssetsCurrent { get; private set; } = new();

    public void Init(AFSModel model, TotalAssets totalAssets)
    {
        Base.BeginningOfyear = model.F1Base.GetF1195Begin() - model.F1Base.F1170.Begin + model.F1Base.F1200.Begin;
        Base.EndOfYear = model.F1Base.GetF1195End() - model.F1Base.F1170.End + model.F1Base.F1200.End;
        Current.BeginningOfyear = model.F1Current.GetF1195Begin() - model.F1Current.F1170.Begin + model.F1Current.F1200.Begin;
        Current.EndOfYear = model.F1Current.GetF1195End() - model.F1Current.F1170.End + model.F1Current.F1200.End;

        InPercentageOfAssetsBase.BeginningOfyear = AFSConstraints.SafeDivide(Base.BeginningOfyear, totalAssets.Base.BeginningOfyear) * 100;
        InPercentageOfAssetsBase.EndOfYear = AFSConstraints.SafeDivide(Base.EndOfYear, totalAssets.Base.EndOfYear) * 100;
        InPercentageOfAssetsCurrent.BeginningOfyear = AFSConstraints.SafeDivide(Current.BeginningOfyear, totalAssets.Current.BeginningOfyear) * 100;
        InPercentageOfAssetsCurrent.EndOfYear = AFSConstraints.SafeDivide(Current.EndOfYear, totalAssets.Current.EndOfYear) * 100;
    }
}

/// <summary>
/// 2.1. Material current assets (Inventory)
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
        Base.BeginningOfyear = model.F1Base.GetAccountsTangibleAssets(true);
        Base.EndOfYear = model.F1Base.GetAccountsTangibleAssets(false);
        Current.BeginningOfyear = model.F1Current.GetAccountsTangibleAssets(true);
        Current.EndOfYear = model.F1Current.GetAccountsTangibleAssets(false);

        InPercentageOfCurrentAssetsBase.BeginningOfyear = AFSConstraints.SafeDivide(Base.BeginningOfyear, currentMobileAssets.Base.BeginningOfyear) * 100;
        InPercentageOfCurrentAssetsBase.EndOfYear = AFSConstraints.SafeDivide(Base.EndOfYear, currentMobileAssets.Base.EndOfYear) * 100;
        InPercentageOfCurrentAssetsCurrent.BeginningOfyear = AFSConstraints.SafeDivide(Current.BeginningOfyear, currentMobileAssets.Current.BeginningOfyear) * 100;
        InPercentageOfCurrentAssetsCurrent.EndOfYear = AFSConstraints.SafeDivide(Current.EndOfYear, currentMobileAssets.Current.EndOfYear) * 100;
    }
}

/// <summary>
/// 2.2. Accounts receivable
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
        Base.BeginningOfyear = model.F1Base.GetAccountsReceivable(true);
        Base.EndOfYear = model.F1Base.GetAccountsReceivable(false);
        Current.BeginningOfyear = model.F1Current.GetAccountsReceivable(true);
        Current.EndOfYear = model.F1Current.GetAccountsReceivable(false);

        InPercentageOfCurrentAssetsBase.BeginningOfyear = AFSConstraints.SafeDivide(Base.BeginningOfyear, currentMobileAssets.Base.BeginningOfyear) * 100;
        InPercentageOfCurrentAssetsBase.EndOfYear = AFSConstraints.SafeDivide(Base.EndOfYear, currentMobileAssets.Base.EndOfYear) * 100;
        InPercentageOfCurrentAssetsCurrent.BeginningOfyear = AFSConstraints.SafeDivide(Current.BeginningOfyear, currentMobileAssets.Current.BeginningOfyear) * 100;
        InPercentageOfCurrentAssetsCurrent.EndOfYear = AFSConstraints.SafeDivide(Current.EndOfYear, currentMobileAssets.Current.EndOfYear) * 100;
    }
}

/// <summary>
/// 2.3. Cash and current financial investments
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
        Base.BeginningOfyear = model.F1Base.GetAccountsMoney(true);
        Base.EndOfYear = model.F1Base.GetAccountsMoney(false);
        Current.BeginningOfyear = model.F1Current.GetAccountsMoney(true);
        Current.EndOfYear = model.F1Current.GetAccountsMoney(false);

        InPercentageOfCurrentAssetsBase.BeginningOfyear = AFSConstraints.SafeDivide(Base.BeginningOfyear, currentMobileAssets.Base.BeginningOfyear) * 100;
        InPercentageOfCurrentAssetsBase.EndOfYear = AFSConstraints.SafeDivide(Base.EndOfYear, currentMobileAssets.Base.EndOfYear) * 100;
        InPercentageOfCurrentAssetsCurrent.BeginningOfyear = AFSConstraints.SafeDivide(Current.BeginningOfyear, currentMobileAssets.Current.BeginningOfyear) * 100;
        InPercentageOfCurrentAssetsCurrent.EndOfYear = AFSConstraints.SafeDivide(Current.EndOfYear, currentMobileAssets.Current.EndOfYear) * 100;
    }
}

/// <summary>
/// 2.4. Other current assets
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
        Base.BeginningOfyear = model.F1Base.F1190.Begin;
        Base.EndOfYear = model.F1Base.F1190.End;
        Current.BeginningOfyear = model.F1Current.F1190.Begin;
        Current.EndOfYear = model.F1Current.F1190.End;

        InPercentageOfCurrentAssetsBase.BeginningOfyear = AFSConstraints.SafeDivide(Base.BeginningOfyear, currentMobileAssets.Base.BeginningOfyear) * 100;
        InPercentageOfCurrentAssetsBase.EndOfYear = AFSConstraints.SafeDivide(Base.EndOfYear, currentMobileAssets.Base.EndOfYear) * 100;
        InPercentageOfCurrentAssetsCurrent.BeginningOfyear = AFSConstraints.SafeDivide(Current.BeginningOfyear, currentMobileAssets.Current.BeginningOfyear) * 100;
        InPercentageOfCurrentAssetsCurrent.EndOfYear = AFSConstraints.SafeDivide(Current.EndOfYear, currentMobileAssets.Current.EndOfYear) * 100;
    }
}

/// <summary>
/// 2.5. Non-current assets held for sale
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
        Base.BeginningOfyear = model.F1Base.F1200.Begin;
        Base.EndOfYear = model.F1Base.F1200.End;
        Current.BeginningOfyear = model.F1Current.F1200.Begin;
        Current.EndOfYear = model.F1Current.F1200.End;

        InPercentageOfCurrentAssetsBase.BeginningOfyear = AFSConstraints.SafeDivide(Base.BeginningOfyear, currentMobileAssets.Base.BeginningOfyear) * 100;
        InPercentageOfCurrentAssetsBase.EndOfYear = AFSConstraints.SafeDivide(Base.EndOfYear, currentMobileAssets.Base.EndOfYear) * 100;
        InPercentageOfCurrentAssetsCurrent.BeginningOfyear = AFSConstraints.SafeDivide(Current.BeginningOfyear, currentMobileAssets.Current.BeginningOfyear) * 100;
        InPercentageOfCurrentAssetsCurrent.EndOfYear = AFSConstraints.SafeDivide(Current.EndOfYear, currentMobileAssets.Current.EndOfYear) * 100;
    }
}

/// <summary>
/// 3. Future period expenses
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
        Base.BeginningOfyear = model.F1Base.F1170.Begin;
        Base.EndOfYear = model.F1Base.F1170.End;
        Current.BeginningOfyear = model.F1Current.F1170.Begin;
        Current.EndOfYear = model.F1Current.F1170.End;

        InPercentageOfAssetsBase.BeginningOfyear = AFSConstraints.SafeDivide(Base.BeginningOfyear, totalAssets.Base.BeginningOfyear) * 100;
        InPercentageOfAssetsBase.EndOfYear = AFSConstraints.SafeDivide(Base.EndOfYear, totalAssets.Base.EndOfYear) * 100;
        InPercentageOfAssetsCurrent.BeginningOfyear = AFSConstraints.SafeDivide(Current.BeginningOfyear, totalAssets.Current.BeginningOfyear) * 100;
        InPercentageOfAssetsCurrent.EndOfYear = AFSConstraints.SafeDivide(Current.EndOfYear, totalAssets.Current.EndOfYear) * 100;
    }
}
