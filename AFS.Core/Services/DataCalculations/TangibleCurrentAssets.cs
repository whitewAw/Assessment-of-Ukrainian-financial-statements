using AFS.Core.Models;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

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

    public void Init(AfsModel model, CurrentMobileAssets currentMobileAssets)
    {
        Base.BeginningOfyear = model.F1Base.GetAccountsTangibleAssets(true);
        Base.EndOfYear = model.F1Base.GetAccountsTangibleAssets(false);
        Current.BeginningOfyear = model.F1Current.GetAccountsTangibleAssets(true);
        Current.EndOfYear = model.F1Current.GetAccountsTangibleAssets(false);

        InPercentageOfCurrentAssetsBase.BeginningOfyear = AfsConstraints.SafeDivide(Base.BeginningOfyear, currentMobileAssets.Base.BeginningOfyear) * 100;
        InPercentageOfCurrentAssetsBase.EndOfYear = AfsConstraints.SafeDivide(Base.EndOfYear, currentMobileAssets.Base.EndOfYear) * 100;
        InPercentageOfCurrentAssetsCurrent.BeginningOfyear = AfsConstraints.SafeDivide(Current.BeginningOfyear, currentMobileAssets.Current.BeginningOfyear) * 100;
        InPercentageOfCurrentAssetsCurrent.EndOfYear = AfsConstraints.SafeDivide(Current.EndOfYear, currentMobileAssets.Current.EndOfYear) * 100;
    }
}
