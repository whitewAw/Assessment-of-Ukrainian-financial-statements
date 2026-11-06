using System.Text.Json.Serialization;

namespace AFS.Core.Json;

public class IntangibleAssetsData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public IntangibleAssetMetricData? NetRevenueFromSales { get; set; }
    public IntangibleAssetMetricData? AverageCostOfIntangibleAssets { get; set; }

    [JsonPropertyName("IntangibleAssetTurnover_UAH")]
    public IntangibleAssetMetricData? IntangibleAssetTurnoverUah { get; set; }

    [JsonPropertyName("CapitalIntensityOfProduction_UAH")]
    public IntangibleAssetMetricData? CapitalIntensityOfProductionUah { get; set; }
}
