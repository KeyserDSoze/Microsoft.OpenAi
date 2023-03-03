using System.Text.Json.Serialization;

namespace Azure.Ai.OpenAi.Completions
{
    public interface IOpenAiRequest
    {
        [JsonPropertyName("model")]
        string? ModelId { get; set; }
    }
}
