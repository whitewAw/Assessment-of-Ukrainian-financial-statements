using System.Text.Json.Serialization;

namespace AFS.Core.Models
{
    /// <summary>
    /// Chart data item for time-series/line charts.
    /// Used with ApexCharts via compile-time lambda expressions (AOT-safe).
    /// </summary>
    public class ChartDateTimeItem
    {
        private double? _value;

        [JsonPropertyName("Date")]
        [JsonInclude]
        public DateTime Date { get; set; }

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
