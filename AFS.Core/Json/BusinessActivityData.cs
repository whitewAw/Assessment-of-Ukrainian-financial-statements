namespace AFS.Core.Json;

public class BusinessActivityData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public BusinessActivityMetricData? GrossProfitMargin { get; set; }
    public BusinessActivityMetricData? BusinessActivityRatio { get; set; }
    public BusinessActivityMetricData? FinancialResourceEfficiency { get; set; }
    public BusinessActivityMetricData? OwnFundsUtilization { get; set; }
    public BusinessActivityMetricData? EnterpriseProfitability { get; set; }
    public BusinessActivityMetricData? LaborProductivity { get; set; }
    public BusinessActivityMetricData? FixedAssetTurnover { get; set; }
    public TurnoverMetricData? ReceivablesTurnover { get; set; }
    public TurnoverMetricData? InventoryTurnover { get; set; }
    public BusinessActivityMetricData? OperatingCycle { get; set; }
    public TurnoverMetricData? CurrentAssetsTurnover { get; set; }
    public BusinessActivityMetricData? EquityTurnover { get; set; }
    public BusinessActivityMetricData? TotalCapitalTurnover { get; set; }
    public BusinessActivityMetricData? EconomicGrowthStability { get; set; }
    public BusinessActivityMetricData? EquityPaybackPeriod { get; set; }
}
