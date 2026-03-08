using System.Text.Json.Serialization;

namespace AFS.Core.Json;

public class GeneralFinancialStabilityData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public StabilitySourceData? OwnWorkingCapital { get; set; }
    public StabilitySourceData? OwnPlusLongTerm { get; set; }
    public StabilitySourceData? TotalAvailable { get; set; }

    [JsonPropertyName("Stocks_Inventory")]
    public StabilitySourceData? StocksInventory { get; set; }

    [JsonPropertyName("Deficit_OwnCapital")]
    public StabilitySourceData? DeficitOwnCapital { get; set; }

    [JsonPropertyName("Deficit_OwnPlusLongTerm")]
    public StabilitySourceData? DeficitOwnPlusLongTerm { get; set; }

    [JsonPropertyName("Deficit_TotalSources")]
    public StabilitySourceData? DeficitTotalSources { get; set; }
}
