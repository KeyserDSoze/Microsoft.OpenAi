using System.Text.Json.Serialization;

namespace Azure.Ai.OpenAi
{
    public sealed class ModerationsRequest : IOpenAiRequest
    {
        [JsonPropertyName("input")]
        public string? Input { get; set; }
        [JsonPropertyName("model")]
        public string? ModelId { get; set; }
    }
}
