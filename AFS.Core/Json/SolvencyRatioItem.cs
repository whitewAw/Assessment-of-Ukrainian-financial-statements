using System.Text.Json.Serialization;

namespace AFS.Core.Json;

public class SolvencyRatioItem
{
    [JsonPropertyName("Base_Begin")]
    public double BaseBegin { get; set; }

    [JsonPropertyName("Base_End")]
    public double BaseEnd { get; set; }

    [JsonPropertyName("Current_Begin")]
    public double CurrentBegin { get; set; }

    [JsonPropertyName("Current_End")]
    public double CurrentEnd { get; set; }

    [JsonPropertyName("Deviation_Base")]
    public double DeviationBase { get; set; }

    [JsonPropertyName("Deviation_Current")]
    public double DeviationCurrent { get; set; }
}
