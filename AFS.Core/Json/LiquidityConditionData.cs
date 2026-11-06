using System.Text.Json.Serialization;

namespace AFS.Core.Json;

public class LiquidityConditionData
{
    public bool IsLiquid { get; set; }

    [JsonPropertyName("A1_MostLiquid")]
    public double A1MostLiquid { get; set; }

    [JsonPropertyName("P1_MostUrgent")]
    public double P1MostUrgent { get; set; }

    [JsonPropertyName("Surplus_A1P1")]
    public double SurplusA1P1 { get; set; }

    [JsonPropertyName("A2_QuickLiquid")]
    public double A2QuickLiquid { get; set; }

    [JsonPropertyName("P2_ShortTerm")]
    public double P2ShortTerm { get; set; }

    [JsonPropertyName("Surplus_A2P2")]
    public double SurplusA2P2 { get; set; }

    [JsonPropertyName("A3_SlowLiquid")]
    public double A3SlowLiquid { get; set; }

    [JsonPropertyName("P3_LongTerm")]
    public double P3LongTerm { get; set; }

    [JsonPropertyName("Surplus_A3P3")]
    public double SurplusA3P3 { get; set; }

    [JsonPropertyName("A4_HardToSell")]
    public double A4HardToSell { get; set; }

    [JsonPropertyName("P4_Permanent")]
    public double P4Permanent { get; set; }

    [JsonPropertyName("Surplus_A4P4")]
    public double SurplusA4P4 { get; set; }
}
