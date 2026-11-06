namespace AFS.Core.Json;

public class FinancialStabilityIndicatorsData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public StabilityIndicatorMetricData? TotalReturnOnAssets { get; set; }
    public StabilityIndicatorMetricData? IndependenceRatio { get; set; }
    public StabilityIndicatorMetricData? FinancialLeverageRatio { get; set; }
    public StabilityIndicatorMetricData? FinancialStabilityRatio { get; set; }
    public StabilityIndicatorMetricData? ManeuverabilityRatio { get; set; }
    public StabilityIndicatorMetricData? BorrowedCapitalConcentration { get; set; }
    public StabilityIndicatorMetricData? LongTermInvestmentStructure { get; set; }
    public StabilityIndicatorMetricData? LongTermBorrowingRatio { get; set; }
    public StabilityIndicatorMetricData? CapitalStructureRatio { get; set; }
    public StabilityIndicatorMetricData? DebtToEquityRatio { get; set; }
    public StabilityIndicatorMetricData? OwnFundsInInventories { get; set; }
    public StabilityIndicatorMetricData? MobileToImmobilizedRatio { get; set; }
    public StabilityIndicatorMetricData? TotalCoverageRatio { get; set; }
}
