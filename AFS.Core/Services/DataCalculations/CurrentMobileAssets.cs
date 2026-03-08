using AFS.Core.Models;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

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

    public void Init(AfsModel model, TotalAssets totalAssets)
    {
        Base.BeginningOfyear = model.F1Base.GetF1195Begin() - model.F1Base.F1170.Begin + model.F1Base.F1200.Begin;
        Base.EndOfYear = model.F1Base.GetF1195End() - model.F1Base.F1170.End + model.F1Base.F1200.End;
        Current.BeginningOfyear = model.F1Current.GetF1195Begin() - model.F1Current.F1170.Begin + model.F1Current.F1200.Begin;
        Current.EndOfYear = model.F1Current.GetF1195End() - model.F1Current.F1170.End + model.F1Current.F1200.End;

        InPercentageOfAssetsBase.BeginningOfyear = AfsConstraints.SafeDivide(Base.BeginningOfyear, totalAssets.Base.BeginningOfyear) * 100;
        InPercentageOfAssetsBase.EndOfYear = AfsConstraints.SafeDivide(Base.EndOfYear, totalAssets.Base.EndOfYear) * 100;
        InPercentageOfAssetsCurrent.BeginningOfyear = AfsConstraints.SafeDivide(Current.BeginningOfyear, totalAssets.Current.BeginningOfyear) * 100;
        InPercentageOfAssetsCurrent.EndOfYear = AfsConstraints.SafeDivide(Current.EndOfYear, totalAssets.Current.EndOfYear) * 100;
    }
}
