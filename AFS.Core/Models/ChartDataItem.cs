using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AFS.Core.Models
{
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties |
     DynamicallyAccessedMemberTypes.PublicFields |
     DynamicallyAccessedMemberTypes.PublicParameterlessConstructor |
     DynamicallyAccessedMemberTypes.PublicMethods)]
    [Serializable]
    public class ChartDataItem
    {
        private double? _value;

        [JsonPropertyName("Item")]
        [JsonInclude]
        [Browsable(true)]
        public string? Item { get; set; }

        [JsonPropertyName("Value")]
        [JsonInclude]
        [Browsable(true)]
        public double? Value
        {
            get => AFSConstraints.RoundStat(_value.GetValueOrDefault(0));
            set => _value = value;
        }
    }
}
