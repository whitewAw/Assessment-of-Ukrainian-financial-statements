using System.Text.Json.Serialization;

namespace AFS.Core.Json;

public class CapitalSourceMetricData
{
    [JsonPropertyName("Base_Begin")]
    public double BaseBegin { get; set; }

    [JsonPropertyName("Base_End")]
    public double BaseEnd { get; set; }

    [JsonPropertyName("Base_Change")]
    public double BaseChange { get; set; }

    [JsonPropertyName("Base_PercentBegin")]
    public double BasePercentBegin { get; set; }

    [JsonPropertyName("Base_PercentEnd")]
    public double BasePercentEnd { get; set; }

    [JsonPropertyName("Current_Begin")]
    public double CurrentBegin { get; set; }

    [JsonPropertyName("Current_End")]
    public double CurrentEnd { get; set; }

    [JsonPropertyName("Current_Change")]
    public double CurrentChange { get; set; }

    [JsonPropertyName("Current_PercentBegin")]
    public double CurrentPercentBegin { get; set; }

    [JsonPropertyName("Current_PercentEnd")]
    public double CurrentPercentEnd { get; set; }
}
