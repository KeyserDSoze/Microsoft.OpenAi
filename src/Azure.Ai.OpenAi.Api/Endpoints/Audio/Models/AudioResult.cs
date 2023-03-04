using System.Text.Json.Serialization;

namespace Azure.Ai.OpenAi.Image
{
    public sealed class AudioResult
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
