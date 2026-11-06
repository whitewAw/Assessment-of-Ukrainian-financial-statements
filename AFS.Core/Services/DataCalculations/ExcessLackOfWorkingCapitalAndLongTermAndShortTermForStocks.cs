using AFS.Core.Interfaces;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

public class ExcessLackOfWorkingCapitalAndLongTermAndShortTermForStocks : IHasBaseCurrent<CharacteristicsOfCapitalCalculationRow>
{
    public string Number { get; private set; } = "7.";
    public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();

    internal void Init(Stocks stocks, AvailabilityForStockFormation availabilityForStockFormation)
    {
        Base.BeginningOfyear = availabilityForStockFormation.Base.BeginningOfyear - stocks.Base.BeginningOfyear;
        Base.EndOfYear = availabilityForStockFormation.Base.EndOfYear - stocks.Base.EndOfYear;

        Current.BeginningOfyear = availabilityForStockFormation.Current.BeginningOfyear - stocks.Current.BeginningOfyear;
        Current.EndOfYear = availabilityForStockFormation.Current.EndOfYear - stocks.Current.EndOfYear;
    }
}
