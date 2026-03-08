using AFS.Core.Interfaces;
using AFS.Core.Models;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

public class FutureIncome : ICapitalSourceMetric<CharacteristicsOfCapitalCalculationRow>
{
    public string Number { get; private set; } = "3.";
    public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow InPercentageOfAssetsBase { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow InPercentageOfAssetsCurrent { get; private set; } = new();

    public void Init(AfsModel model)
    {
        Base.BeginningOfyear = model.F1Base.F1665.Begin;
        Base.EndOfYear = model.F1Base.F1665.End;

        Current.BeginningOfyear = model.F1Current.F1665.Begin;
        Current.EndOfYear = model.F1Current.F1665.End;

        InPercentageOfAssetsBase.BeginningOfyear = Base.BeginningOfyear / model.F1Base.GetF1900Begin() * 100;
        InPercentageOfAssetsBase.EndOfYear = Base.EndOfYear / model.F1Base.GetF1900End() * 100;

        InPercentageOfAssetsCurrent.BeginningOfyear = Current.BeginningOfyear / model.F1Current.GetF1900Begin() * 100;
        InPercentageOfAssetsCurrent.EndOfYear = Current.EndOfYear / model.F1Current.GetF1900End() * 100;
    }
}
