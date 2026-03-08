using System.Text.Json.Serialization;

namespace AFS.Core.Json;

public class ReceivablePayableCategoryData
{
    [JsonPropertyName("Receivable_Base")]
    public double ReceivableBase { get; set; }

    [JsonPropertyName("Receivable_Current")]
    public double ReceivableCurrent { get; set; }

    [JsonPropertyName("Payable_Base")]
    public double PayableBase { get; set; }

    [JsonPropertyName("Payable_Current")]
    public double PayableCurrent { get; set; }

    [JsonPropertyName("ExcessReceivable_Base")]
    public double ExcessReceivableBase { get; set; }

    [JsonPropertyName("ExcessReceivable_Current")]
    public double ExcessReceivableCurrent { get; set; }

    [JsonPropertyName("ExcessPayable_Base")]
    public double ExcessPayableBase { get; set; }

    [JsonPropertyName("ExcessPayable_Current")]
    public double ExcessPayableCurrent { get; set; }
}
