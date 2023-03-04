using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Azure.Ai.OpenAi.Image
{
    public sealed class ImageResult : ApiBaseResponse
    {
        [JsonPropertyName("data")]
        public List<ImageData>? Data { get; set; }
    }
}
