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

    public void Init(AfsModel model)
    {
        // Base Year (2019)
        Base.BeginningOfyear = model.F1Base.GetF1300Begin();
        Base.EndOfYear = model.F1Base.GetF1300End();

        // Current Year (2020)
        Current.BeginningOfyear = model.F1Current.GetF1300Begin();
        Current.EndOfYear = model.F1Current.GetF1300End();
    }
}
