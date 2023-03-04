using System.Text.Json.Serialization;

namespace OpenSource.OpenAi.Moderation
{
    public sealed class ModerationsRequest : IOpenAiRequest
    {
        [JsonPropertyName("input")]
        public string? Input { get; set; }
        [JsonPropertyName("model")]
        public string? ModelId { get; set; }
    }
}
