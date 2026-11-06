using AFS.Core.Models;

namespace AFS.Core.Services.DataCalculations;

public class CharacteristicsOfCapital
{
    public TotalAssets TotalAssets { get; private set; } = new();
    public NonCurrentImmobilizedFunds NonCurrentImmobilizedFunds { get; private set; } = new();
    public CurrentMobileAssets CurrentMobileAssets { get; private set; } = new();
    public TangibleCurrentAssets TangibleCurrentAssets { get; private set; } = new();
    public AccountsReceivable AccountsReceivable { get; private set; } = new();
    public CashCurrentFinancialInvestments CashCurrentFinancialInvestments { get; private set; } = new();
    public OtherCurrentAssets OtherCurrentAssets { get; private set; } = new();
    public NonCurrentAssetsHeldForSale NonCurrentAssetsHeldForSale { get; private set; } = new();
    public FutureExpenses FutureExpenses { get; private set; } = new();

    public CharacteristicsOfCapital(AfsModel model) => Init(model);

    private void Init(AfsModel model)
    {
        TotalAssets.Init(model);
        NonCurrentImmobilizedFunds.Init(model, TotalAssets);
        CurrentMobileAssets.Init(model, TotalAssets);
        TangibleCurrentAssets.Init(model, CurrentMobileAssets);
        AccountsReceivable.Init(model, CurrentMobileAssets);
        CashCurrentFinancialInvestments.Init(model, CurrentMobileAssets);
        OtherCurrentAssets.Init(model, CurrentMobileAssets);
        NonCurrentAssetsHeldForSale.Init(model, CurrentMobileAssets);
        FutureExpenses.Init(model, TotalAssets);
    }
}
