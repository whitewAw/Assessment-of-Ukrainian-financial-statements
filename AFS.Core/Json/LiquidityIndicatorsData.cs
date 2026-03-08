using System.Text.Json.Serialization;

namespace AFS.Core.Json;

public class LiquidityIndicatorsData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }

    [JsonPropertyName("BaseYear_Assessment")]
    public LiquidityPeriodData? BaseYearAssessment { get; set; }

    [JsonPropertyName("CurrentYear_Assessment")]
    public LiquidityPeriodData? CurrentYearAssessment { get; set; }
}
