﻿using System.Text.Json.Serialization;

namespace Azure.Ai.OpenAi.Image
{
    public sealed class AudioTranscriptionRequest : IOpenAiRequest
    {
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }
        [JsonPropertyName("n")]
        public int NumberOfResults { get; set; }
        [JsonPropertyName("size")]
        public string Size { get; set; }
        [JsonPropertyName("user")]
        public string User { get; set; }
        [JsonPropertyName("response_format")]
        public string ResponseFormat { get; set; }
        [JsonIgnore]
        public string? ModelId { get; set; }
    }
}