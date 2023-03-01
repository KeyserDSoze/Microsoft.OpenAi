using System.Text.Json.Serialization;

namespace Azure.Ai.OpenAi
{
    public sealed class ModerationResult
    {
        [JsonPropertyName("categories")]
        public Categories Categories { get; set; }

        [JsonPropertyName("category_scores")]
        public Scores Scores { get; set; }

        [JsonPropertyName("flagged")]
        public bool Flagged { get; set; }
    }
}
