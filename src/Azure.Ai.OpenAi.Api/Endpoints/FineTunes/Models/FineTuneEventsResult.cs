using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Azure.Ai.OpenAi.FineTune
{
    public sealed class FineTuneEventsResult
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }
        [JsonPropertyName("data")]
        public List<FineTuneEvent> Results { get; set; }
    }

}
