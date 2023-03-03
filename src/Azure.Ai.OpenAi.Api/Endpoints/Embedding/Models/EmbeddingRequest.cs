using System.Text.Json.Serialization;

namespace Azure.Ai.OpenAi.Embedding
{
    public sealed class EmbeddingRequest : IOpenAiRequest
    {
        [JsonPropertyName("model")]
        public string ModelId { get; set; }
        [JsonPropertyName("input")]
        public object Input { get; set; }
        [JsonPropertyName("user")]
        public object User { get; set; }
    }
}
