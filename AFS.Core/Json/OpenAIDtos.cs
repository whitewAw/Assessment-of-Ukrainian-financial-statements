using System.Text.Json.Serialization;

namespace AFS.Core.Json;

/// <summary>
/// DTOs for OpenAI API communication.
/// </summary>
public class OpenAIMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}

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

public class OpenAIChoice
{
    [JsonPropertyName("message")]
    public OpenAIMessage? Message { get; set; }
}

public class OpenAIResponse
{
    [JsonPropertyName("choices")]
    public OpenAIChoice[] Choices { get; set; } = Array.Empty<OpenAIChoice>();
}
