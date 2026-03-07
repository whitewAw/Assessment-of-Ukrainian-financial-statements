using System.Text.Json.Serialization;

namespace AFS.Core.Models
{
    /// <summary>
    /// Chart data item for pie/donut charts.
    /// Used with ApexCharts via compile-time lambda expressions (AOT-safe).
    /// </summary>
    public class ChartDataItem
    {
        private double? _value;

        [JsonPropertyName("Item")]
        [JsonInclude]
        public string? Item { get; set; }

        [JsonPropertyName("Value")]
        [JsonInclude]
        public double? Value
        {
            get => AFSConstraints.RoundStat(_value.GetValueOrDefault(0));
            set => _value = value;
        }
    }
}
