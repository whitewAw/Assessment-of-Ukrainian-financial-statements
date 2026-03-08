using System.Text.Json.Serialization;

namespace AFS.Core.Json;

public class ReceivablePayableSummary
{
    [JsonPropertyName("TotalReceivables_Base")]
    public double TotalReceivablesBase { get; set; }

    [JsonPropertyName("TotalReceivables_Current")]
    public double TotalReceivablesCurrent { get; set; }

    [JsonPropertyName("TotalPayables_Base")]
    public double TotalPayablesBase { get; set; }

    [JsonPropertyName("TotalPayables_Current")]
    public double TotalPayablesCurrent { get; set; }

    [JsonPropertyName("NetPosition_Base")]
    public double NetPositionBase { get; set; }

    [JsonPropertyName("NetPosition_Current")]
    public double NetPositionCurrent { get; set; }

    [JsonPropertyName("ReceivableToPayableRatio_Base")]
    public double ReceivableToPayableRatioBase { get; set; }

    [JsonPropertyName("ReceivableToPayableRatio_Current")]
    public double ReceivableToPayableRatioCurrent { get; set; }
}
