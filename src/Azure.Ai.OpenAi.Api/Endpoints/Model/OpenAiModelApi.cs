using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Ai.OpenAi.Models
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
        public ValueTask<Model> RetrieveAsync(string id, CancellationToken cancellationToken = default)
            => _client.ExecuteAsync<Model>($"{_configuration.GetUri(OpenAi.Model, string.Empty)}/{id}", null, cancellationToken);
        public async Task<List<Model>> ListAsync(CancellationToken cancellationToken = default)
        {
            var response = await _client.ExecuteAsync<JsonHelperRoot>(_configuration.GetUri(OpenAi.Model, string.Empty), null, cancellationToken);
            return response.Data!;
        }
        private sealed class JsonHelperRoot : ApiBaseResponse
        {
            [JsonPropertyName("data")]
            public List<Model>? Data { get; set; }
        }
    }
}
