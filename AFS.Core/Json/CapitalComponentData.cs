using System.Text.Json.Serialization;

namespace AFS.Core.Json;

public class CapitalComponentData
{
    [JsonPropertyName("Base_End")]
    public double BaseEnd { get; set; }

    [JsonPropertyName("Base_Percent")]
    public double BasePercent { get; set; }

    [JsonPropertyName("Current_End")]
    public double CurrentEnd { get; set; }

    [JsonPropertyName("Current_Percent")]
    public double CurrentPercent { get; set; }
}
