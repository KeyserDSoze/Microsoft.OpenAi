using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Azure.Ai.OpenAi.Models;

namespace Azure.Ai.OpenAi
{
    internal sealed class OpenAiModelApi : IOpenAiModelApi
    {
        private readonly HttpClient _client;
        private readonly OpenAiConfiguration _configuration;
        public OpenAiModelApi(IHttpClientFactory httpClientFactory, OpenAiConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient(OpenAiSettings.HttpClientName);
            _configuration = configuration;
        }
        public ValueTask<Model> GetDetailsAsync(string id, CancellationToken cancellationToken = default)
            => _client.ExecuteAsync<Model>($"{_configuration.ModelUri}/{id}", null, cancellationToken);
        public async Task<List<Model>> AllAsync(CancellationToken cancellationToken = default)
        {
            var response = await _client.ExecuteAsync<JsonHelperRoot>(_configuration.ModelUri, null, cancellationToken);
            return response.Data!;
        }
        private sealed class JsonHelperRoot : ApiBaseResponse
        {
            [JsonPropertyName("data")]
            public List<Model>? Data { get; set; }
            [JsonPropertyName("object")]
            public string? Object { get; set; }
        }
    }
}
