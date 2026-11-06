using AFS.Core.Models;

namespace AFS.Core.Services.DataCalculations;

public class GeneralIndicatorsOfFinancialStability
{
    public AvailabilityOfWorkingCapitalForFormationOfStocks AvailabilityOfWorkingCapitalForFormationOfStocks { get; private set; } = new();
    public AvailabilityOfOwnCurrentAndLongTermBorrowedForStocks AvailabilityOfOwnCurrentAndLongTermBorrowedForStocks { get; private set; } = new();
    public AvailabilityForStockFormation AvailabilityForStockFormation { get; private set; } = new();
    public Stocks Stocks { get; private set; } = new();
    public ExcessLackOfWorkingCapitalForStocks ExcessLackOfWorkingCapitalForStocks { get; private set; } = new();
    public ExcessLackOfWorkingCapitalAndLongTermForStocks ExcessLackOfWorkingCapitalAndLongTermForStocks { get; private set; } = new();
    public ExcessLackOfWorkingCapitalAndLongTermAndShortTermForStocks ExcessLackOfWorkingCapitalAndLongTermAndShortTermForStocks { get; private set; } = new();

    public GeneralIndicatorsOfFinancialStability(AfsModel model) => Init(model);

    private void Init(AfsModel model)
    {
        SourcesOfCapitalFormation sOCF = new(model);

        AvailabilityOfWorkingCapitalForFormationOfStocks.Init(sOCF);
        AvailabilityOfOwnCurrentAndLongTermBorrowedForStocks.Init(model);
        AvailabilityForStockFormation.Init(model);
        Stocks.Init(model);
        ExcessLackOfWorkingCapitalForStocks.Init(Stocks, AvailabilityOfWorkingCapitalForFormationOfStocks);
        ExcessLackOfWorkingCapitalAndLongTermForStocks.Init(Stocks, AvailabilityOfOwnCurrentAndLongTermBorrowedForStocks);
        ExcessLackOfWorkingCapitalAndLongTermAndShortTermForStocks.Init(Stocks, AvailabilityForStockFormation);
    }
}
