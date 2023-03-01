using System.Collections.Generic;
using System.Text.Json.Serialization;
using Azure.Ai.OpenAi;

namespace Microsoft.OpenAi.Api.Endpoints.Image.Models
{
    public sealed class ImagesResponse : ApiBaseResponse
    {
        [JsonPropertyName("created")]
        public int Created { get; set; }
        [JsonPropertyName("data")]
        public List<ImageResult> Data { get; set; }
    }
}
