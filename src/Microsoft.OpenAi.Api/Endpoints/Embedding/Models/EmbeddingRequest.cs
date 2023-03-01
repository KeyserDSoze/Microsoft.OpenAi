using System.Text.Json.Serialization;

namespace Microsoft.OpenAi.Api.Embedding
{
    internal sealed class EmbeddingRequest
    {
        [JsonPropertyName("model")]
        public string ModelId { get; set; }
        [JsonPropertyName("input")]
        public string Input { get; set; }
    }
}
