using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OpenSource.OpenAi.FineTune
{
    public sealed class FineTuneResults : ApiBaseResponse
    {
        [JsonPropertyName("data")]
        public List<FineTuneResult>? Data { get; set; }
    }
}
