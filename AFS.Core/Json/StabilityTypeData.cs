using System.Text.Json.Serialization;

namespace AFS.Core.Json;

public class StabilityTypeData
{
    public string? Type { get; set; }

    [JsonPropertyName("Base_Current")]
    public bool BaseCurrent { get; set; }

    [JsonPropertyName("Base_ShortTerm")]
    public bool BaseShortTerm { get; set; }

    [JsonPropertyName("Base_LongTerm")]
    public bool BaseLongTerm { get; set; }

    [JsonPropertyName("Current_Current")]
    public bool CurrentCurrent { get; set; }

    [JsonPropertyName("Current_ShortTerm")]
    public bool CurrentShortTerm { get; set; }

    [JsonPropertyName("Current_LongTerm")]
    public bool CurrentLongTerm { get; set; }
}
