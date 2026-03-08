using AFS.Core.Interfaces;
using AFS.Core.Models;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

public class AvailabilityForStockFormation : IHasBaseCurrent<CharacteristicsOfCapitalCalculationRow>
{
    public string Number { get; private set; } = "3.";
    public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();

    public void Init(AfsModel model)
    {
        Base.BeginningOfyear = model.F1Base.GetF1495Begin() + model.F1Base.GetF1595Begin() - model.F1Base.GetF1095Begin() + model.F1Base.F1600.Begin + model.F1Base.F1610.Begin + model.F1Base.F1660.Begin;
        Base.EndOfYear = model.F1Base.GetF1495End() + model.F1Base.GetF1595End() - model.F1Base.GetF1095End() + model.F1Base.F1600.End + model.F1Base.F1610.End + model.F1Base.F1660.End;

        Current.BeginningOfyear = model.F1Current.GetF1495Begin() + model.F1Current.GetF1595Begin() - model.F1Current.GetF1095Begin() + model.F1Current.F1600.Begin + model.F1Current.F1610.Begin + model.F1Current.F1660.Begin;
        Current.EndOfYear = model.F1Current.GetF1495End() + model.F1Current.GetF1595End() - model.F1Current.GetF1095End() + model.F1Current.F1600.End + model.F1Current.F1610.End + model.F1Current.F1660.End;
    }
}
