using System.Text.Json.Serialization;

namespace AFS.Core.Json;

public class SolvencyRatioSimpleItem
{
    [JsonPropertyName("Base_End")]
    public double BaseEnd { get; set; }

    [JsonPropertyName("Current_End")]
    public double CurrentEnd { get; set; }

    public double Deviation { get; set; }
}
