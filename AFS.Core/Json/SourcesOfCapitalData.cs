namespace AFS.Core.Json;

public class SourcesOfCapitalData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public CapitalSourceMetricData? TotalCapital { get; set; }
    public CapitalSourceMetricData? Equity { get; set; }
    public CapitalComponentData? OwnCurrentAssets { get; set; }
    public CapitalSourceMetricData? BorrowedCapital { get; set; }
    public CapitalComponentData? LongTermLiabilities { get; set; }
    public CapitalComponentData? ShortTermLoans { get; set; }
    public CapitalComponentData? AccountsPayable { get; set; }
    public CapitalComponentData? OtherCurrentLiabilities { get; set; }
}
