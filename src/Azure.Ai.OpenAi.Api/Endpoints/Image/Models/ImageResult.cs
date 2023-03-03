using System.Text.Json.Serialization;

namespace Azure.Ai.OpenAi.Image
{
    public sealed class ImageData
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
