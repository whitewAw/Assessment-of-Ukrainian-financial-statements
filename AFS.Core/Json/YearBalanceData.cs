namespace AFS.Core.Json;

public class YearBalanceData
{
    public BalanceItemData? TotalAssets { get; set; }
    public BalanceItemData? NonCurrentAssets { get; set; }
    public BalanceItemData? CurrentAssets { get; set; }
    public BalanceItemData? Equity { get; set; }
    public BalanceItemData? TotalLiabilities { get; set; }
    public BalanceItemData? CurrentLiabilities { get; set; }
}
