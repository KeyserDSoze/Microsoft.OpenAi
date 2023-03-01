using System.Text.Json.Serialization;

namespace Azure.Ai.OpenAi
{
    public sealed class ImageResult
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
