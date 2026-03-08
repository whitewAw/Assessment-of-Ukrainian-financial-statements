using AFS.Core.Interfaces;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

public class ExcessLackOfWorkingCapitalAndLongTermForStocks : IHasBaseCurrent<CharacteristicsOfCapitalCalculationRow>
{
    public string Number { get; private set; } = "6.";
    public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();

    internal void Init(Stocks stocks, AvailabilityOfOwnCurrentAndLongTermBorrowedForStocks availabilityOfOwnCurrentAndLongTermBorrowedForStocks)
    {
        Base.BeginningOfyear = availabilityOfOwnCurrentAndLongTermBorrowedForStocks.Base.BeginningOfyear - stocks.Base.BeginningOfyear;
        Base.EndOfYear = availabilityOfOwnCurrentAndLongTermBorrowedForStocks.Base.EndOfYear - stocks.Base.EndOfYear;

        Current.BeginningOfyear = availabilityOfOwnCurrentAndLongTermBorrowedForStocks.Current.BeginningOfyear - stocks.Current.BeginningOfyear;
        Current.EndOfYear = availabilityOfOwnCurrentAndLongTermBorrowedForStocks.Current.EndOfYear - stocks.Current.EndOfYear;
    }
}
