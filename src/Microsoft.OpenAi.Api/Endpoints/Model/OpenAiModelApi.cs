using System.Text.Json.Serialization;
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
        /// <summary>
        /// Get details about a particular Model from the API, specifically properties such as <see cref="Model.OwnedBy"/> and permissions.
        /// </summary>
        /// <param name="id">The id/name of the model to get more details about</param>
        /// <returns>Asynchronously returns the <see cref="Model"/> with all available properties</returns>
        public ValueTask<Model> GetDetailsAsync(string id, CancellationToken cancellationToken = default)
            => _client.ExecuteAsync<Model>($"{_configuration.ModelUri}/{id}", null, cancellationToken);
        /// <summary>
        /// List all models via the API
        /// </summary>
        /// <returns>Asynchronously returns the list of all <see cref="Model"/>s</returns>
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
