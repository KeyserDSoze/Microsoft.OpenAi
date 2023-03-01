using Microsoft.OpenAi.Api.Completions;

namespace Microsoft.OpenAi.Api
{
    internal sealed class OpenAiEmbeddingApi : IOpenAiEmbeddingApi
    {
        private readonly HttpClient _client;
        private readonly OpenAiConfiguration _configuration;
        public OpenAiEmbeddingApi(IHttpClientFactory httpClientFactory, OpenAiConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient(OpenAiSettings.HttpClientName);
            _configuration = configuration;
        }
        public EmbeddingRequestBuilder Request(string input)
            => new EmbeddingRequestBuilder(_client, _configuration, input);
    }
}
