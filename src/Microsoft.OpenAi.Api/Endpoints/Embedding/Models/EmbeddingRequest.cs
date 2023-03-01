using System.Text.Json.Serialization;

namespace Azure.Ai.OpenAi.Embedding
{
    internal sealed class EmbeddingRequest
    {
        [JsonPropertyName("model")]
        public string ModelId { get; set; }
        [JsonPropertyName("input")]
        public string Input { get; set; }
    }
}
