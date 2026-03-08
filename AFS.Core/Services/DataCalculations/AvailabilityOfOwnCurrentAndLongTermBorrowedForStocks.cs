using AFS.Core.Interfaces;
using AFS.Core.Models;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

public class AvailabilityOfOwnCurrentAndLongTermBorrowedForStocks : IHasBaseCurrent<CharacteristicsOfCapitalCalculationRow>
{
    public string Number { get; private set; } = "2.";
    public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();

    public void Init(AfsModel model)
    {
        Base.BeginningOfyear = model.F1Base.GetF1495Begin() + model.F1Base.GetF1595Begin() - model.F1Base.GetF1095Begin();
        Base.EndOfYear = model.F1Base.GetF1495End() + model.F1Base.GetF1595End() - model.F1Base.GetF1095End();

        Current.BeginningOfyear = model.F1Current.GetF1495Begin() + model.F1Current.GetF1595Begin() - model.F1Current.GetF1095Begin();
        Current.EndOfYear = model.F1Current.GetF1495End() + model.F1Current.GetF1595End() - model.F1Current.GetF1095End();
    }
}
