using System.Text.Json.Serialization;

namespace OpenSource.OpenAi.FineTune
{
    public class FineTuneDeleteResult
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("object")]
        public string? Object { get; set; }
        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }
    }

}
