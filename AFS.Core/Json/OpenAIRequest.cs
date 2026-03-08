using System.Text.Json.Serialization;

namespace AFS.Core.Json;

public class OpenAIRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("messages")]
    public OpenAIMessage[] Messages { get; set; } = Array.Empty<OpenAIMessage>();

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; }
}
