using System.Text.Json.Serialization;

namespace AFS.Core.Json;

public class OpenAIResponse
{
    [JsonPropertyName("choices")]
    public OpenAIChoice[] Choices { get; set; } = Array.Empty<OpenAIChoice>();
}
