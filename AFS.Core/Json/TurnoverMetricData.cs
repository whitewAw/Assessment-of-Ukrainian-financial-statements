using System.Text.Json.Serialization;

namespace AFS.Core.Json;

public class TurnoverMetricData
{
    [JsonPropertyName("Revolutions_Base")]
    public double RevolutionsBase { get; set; }

    [JsonPropertyName("Revolutions_Current")]
    public double RevolutionsCurrent { get; set; }

    [JsonPropertyName("Days_Base")]
    public double DaysBase { get; set; }

    [JsonPropertyName("Days_Current")]
    public double DaysCurrent { get; set; }
}
