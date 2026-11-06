using AFS.Core.Models;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

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

    public void Init(AfsModel model, CurrentMobileAssets currentMobileAssets)
    {
        Base.BeginningOfyear = model.F1Base.GetAccountsMoney(true);
        Base.EndOfYear = model.F1Base.GetAccountsMoney(false);
        Current.BeginningOfyear = model.F1Current.GetAccountsMoney(true);
        Current.EndOfYear = model.F1Current.GetAccountsMoney(false);

        InPercentageOfCurrentAssetsBase.BeginningOfyear = AfsConstraints.SafeDivide(Base.BeginningOfyear, currentMobileAssets.Base.BeginningOfyear) * 100;
        InPercentageOfCurrentAssetsBase.EndOfYear = AfsConstraints.SafeDivide(Base.EndOfYear, currentMobileAssets.Base.EndOfYear) * 100;
        InPercentageOfCurrentAssetsCurrent.BeginningOfyear = AfsConstraints.SafeDivide(Current.BeginningOfyear, currentMobileAssets.Current.BeginningOfyear) * 100;
        InPercentageOfCurrentAssetsCurrent.EndOfYear = AfsConstraints.SafeDivide(Current.EndOfYear, currentMobileAssets.Current.EndOfYear) * 100;
    }
}
