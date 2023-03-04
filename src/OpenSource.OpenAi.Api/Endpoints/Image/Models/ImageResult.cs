using System.Text.Json.Serialization;

namespace OpenSource.OpenAi.Image
{
    public sealed class ImageData
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}
