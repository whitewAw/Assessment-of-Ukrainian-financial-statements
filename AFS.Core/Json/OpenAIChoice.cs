using System.Text.Json.Serialization;

namespace AFS.Core.Json;

public class OpenAIChoice
{
    [JsonPropertyName("message")]
    public OpenAIMessage? Message { get; set; }
}
