using AFS.Core.Models;

namespace AFS.Core.Services.DataCalculations;

public class SourcesOfCapitalFormation
{
    public TotalSourcesOfCapital TotalSourcesOfCapital { get; private set; } = new();
    public Equity Equity { get; private set; } = new();
    public OwnCurrentAssets OwnCurrentAssets { get; private set; } = new();
    public RaisedCapital RaisedCapital { get; private set; } = new();
    public LongTermLiabilities LongTermLiabilities { get; private set; } = new();
    public ShortTermLoans ShortTermLoans { get; private set; } = new();
    public AccountsPayable AccountsPayable { get; private set; } = new();
    public OtherCurrentLiabilities OtherCurrentLiabilities { get; private set; } = new();
    public LiabilitiesRelatedNonCurrentAssetsHeldForSale LiabilitiesRelatedNonCurrentAssetsHeldForSale { get; private set; } = new();
    public FutureIncome FutureIncome { get; private set; } = new();

    public SourcesOfCapitalFormation(AfsModel model) => Init(model);

    private void Init(AfsModel model)
    {
        TotalSourcesOfCapital.Init(model);
        Equity.Init(model);
        OwnCurrentAssets.Init(model, Equity);
        RaisedCapital.Init(model);
        LongTermLiabilities.Init(model, RaisedCapital);
        ShortTermLoans.Init(model, RaisedCapital);
        AccountsPayable.Init(model, RaisedCapital);
        OtherCurrentLiabilities.Init(model, RaisedCapital);
        LiabilitiesRelatedNonCurrentAssetsHeldForSale.Init(model, RaisedCapital);
        FutureIncome.Init(model);
    }
}
