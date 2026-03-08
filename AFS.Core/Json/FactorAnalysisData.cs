namespace AFS.Core.Json;

public class FactorAnalysisData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public FactorMetricData? NetRevenueFromSales { get; set; }
    public FactorMetricData? AverageNumberOfEmployees { get; set; }
    public FactorMetricData? LaborProductivity { get; set; }
    public FactorMetricData? AverageCostOfFixedAssets { get; set; }
    public FactorMetricData? CapitalIntensity { get; set; }
    public FactorMetricData? FixedAssetTurnover { get; set; }
}
