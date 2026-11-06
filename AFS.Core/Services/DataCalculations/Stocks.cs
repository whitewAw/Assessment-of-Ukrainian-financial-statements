using AFS.Core.Interfaces;
using AFS.Core.Models;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

public class Stocks : IHasBaseCurrent<CharacteristicsOfCapitalCalculationRow>
{
    public string Number { get; private set; } = "4.";
    public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();

    public void Init(AfsModel model)
    {
        Base.BeginningOfyear = model.F1Base.GetAccountsTangibleAssets(true);
        Base.EndOfYear = model.F1Base.GetAccountsTangibleAssets(false);

        Current.BeginningOfyear = model.F1Current.GetAccountsTangibleAssets(true);
        Current.EndOfYear = model.F1Current.GetAccountsTangibleAssets(false);
    }
}
