using AFS.Core.Models;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

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

    public void Init(AfsModel model, TotalAssets totalAssets)
    {
        Base.BeginningOfyear = model.F1Base.GetF1095Begin();
        Base.EndOfYear = model.F1Base.GetF1095End();
        Current.BeginningOfyear = model.F1Current.GetF1095Begin();
        Current.EndOfYear = model.F1Current.GetF1095End();

        InPercentageOfAssetsBase.BeginningOfyear = AfsConstraints.SafeDivide(Base.BeginningOfyear, totalAssets.Base.BeginningOfyear) * 100;
        InPercentageOfAssetsBase.EndOfYear = AfsConstraints.SafeDivide(Base.EndOfYear, totalAssets.Base.EndOfYear) * 100;
        InPercentageOfAssetsCurrent.BeginningOfyear = AfsConstraints.SafeDivide(Current.BeginningOfyear, totalAssets.Current.BeginningOfyear) * 100;
        InPercentageOfAssetsCurrent.EndOfYear = AfsConstraints.SafeDivide(Current.EndOfYear, totalAssets.Current.EndOfYear) * 100;
    }
}
