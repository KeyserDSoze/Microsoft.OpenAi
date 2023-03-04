using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Azure.Ai.OpenAi.FineTune
{
    public sealed class FineTuneEventsResult : ApiBaseResponse
    {
        [JsonPropertyName("data")]
        public List<FineTuneEvent>? Data { get; set; }
    }

}
