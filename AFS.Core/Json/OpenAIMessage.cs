using System.Text.Json.Serialization;

namespace AFS.Core.Json;

/// <summary>
/// Message for OpenAI API communication.
/// </summary>
public class OpenAIMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}
