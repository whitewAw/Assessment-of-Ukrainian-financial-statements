using AFS.Core.Interfaces;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

public class ExcessLackOfWorkingCapitalForStocks : IHasBaseCurrent<CharacteristicsOfCapitalCalculationRow>
{
    public string Number { get; private set; } = "5.";
    public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();

    internal void Init(Stocks stocks, AvailabilityOfWorkingCapitalForFormationOfStocks availabilityOfWorkingCapitalForFormationOfStocks)
    {
        Base.BeginningOfyear = availabilityOfWorkingCapitalForFormationOfStocks.Base.BeginningOfyear - stocks.Base.BeginningOfyear;
        Base.EndOfYear = availabilityOfWorkingCapitalForFormationOfStocks.Base.EndOfYear - stocks.Base.EndOfYear;

        Current.BeginningOfyear = availabilityOfWorkingCapitalForFormationOfStocks.Current.BeginningOfyear - stocks.Current.BeginningOfyear;
        Current.EndOfYear = availabilityOfWorkingCapitalForFormationOfStocks.Current.EndOfYear - stocks.Current.EndOfYear;
    }
}
