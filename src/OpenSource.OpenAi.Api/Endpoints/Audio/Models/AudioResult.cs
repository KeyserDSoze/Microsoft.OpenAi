using System.Text.Json.Serialization;

namespace OpenSource.OpenAi.Image
{
    public sealed class AudioResult
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
