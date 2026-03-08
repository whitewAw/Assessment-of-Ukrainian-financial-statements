using AFS.Core.Models;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

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

    public void Init(AfsModel model, CurrentMobileAssets currentMobileAssets)
    {
        Base.BeginningOfyear = model.F1Base.F1190.Begin;
        Base.EndOfYear = model.F1Base.F1190.End;
        Current.BeginningOfyear = model.F1Current.F1190.Begin;
        Current.EndOfYear = model.F1Current.F1190.End;

        InPercentageOfCurrentAssetsBase.BeginningOfyear = AfsConstraints.SafeDivide(Base.BeginningOfyear, currentMobileAssets.Base.BeginningOfyear) * 100;
        InPercentageOfCurrentAssetsBase.EndOfYear = AfsConstraints.SafeDivide(Base.EndOfYear, currentMobileAssets.Base.EndOfYear) * 100;
        InPercentageOfCurrentAssetsCurrent.BeginningOfyear = AfsConstraints.SafeDivide(Current.BeginningOfyear, currentMobileAssets.Current.BeginningOfyear) * 100;
        InPercentageOfCurrentAssetsCurrent.EndOfYear = AfsConstraints.SafeDivide(Current.EndOfYear, currentMobileAssets.Current.EndOfYear) * 100;
    }
}
