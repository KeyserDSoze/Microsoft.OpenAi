using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Azure.Ai.OpenAi.FineTune
{
    public sealed class FineTuneResults
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }
        [JsonPropertyName("data")]
        public List<FineTuneResult> Results { get; set; }
    }
}
