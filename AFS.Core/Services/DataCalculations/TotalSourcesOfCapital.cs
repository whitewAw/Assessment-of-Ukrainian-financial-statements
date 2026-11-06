using AFS.Core.Interfaces;
using AFS.Core.Models;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

public class TotalSourcesOfCapital : IHasBaseCurrent<CharacteristicsOfCapitalCalculationRow>
{
    public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();

    public void Init(AfsModel model)
    {
        Base.BeginningOfyear = model.F1Base.GetF1900Begin();
        Base.EndOfYear = model.F1Base.GetF1900End();

        Current.BeginningOfyear = model.F1Current.GetF1900Begin();
        Current.EndOfYear = model.F1Current.GetF1900End();
    }
}
