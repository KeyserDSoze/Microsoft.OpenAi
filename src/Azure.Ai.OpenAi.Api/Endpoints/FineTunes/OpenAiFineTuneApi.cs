using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Ai.OpenAi.FineTune
{
    internal sealed class OpenAiFineTuneApi : IOpenAiFineTuneApi
    {
        private readonly HttpClient _client;
        private readonly OpenAiConfiguration _configuration;
        public OpenAiFineTuneApi(IHttpClientFactory httpClientFactory, OpenAiConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient(OpenAiSettings.HttpClientName);
            _configuration = configuration;
        }
        public FineTuneRequestBuilder Create(string trainingFileId)
            => new FineTuneRequestBuilder(_client, _configuration, trainingFileId);
        public ValueTask<FineTuneResults> ListAsync(string fineTuneId, CancellationToken cancellationToken = default)
            => _client.ExecuteAsync<FineTuneResults>(_configuration.GetUri(OpenAi.FineTune, fineTuneId), null, cancellationToken);
        public ValueTask<FineTuneResult> RetrieveAsync(string fineTuneId, CancellationToken cancellationToken = default)
            => _client.ExecuteAsync<FineTuneResult>($"{_configuration.GetUri(OpenAi.FineTune, fineTuneId)}/{fineTuneId}", null, cancellationToken);
        public ValueTask<FineTuneResult> CancelAsync(string fineTuneId, CancellationToken cancellationToken = default)
            => _client.ExecuteAsync<FineTuneResult>($"{_configuration.GetUri(OpenAi.FineTune, fineTuneId)}/{fineTuneId}/cancel", null, cancellationToken, true);
        public ValueTask<FineTuneEventsResult> ListEventsAsync(string fineTuneId, CancellationToken cancellationToken = default)
            => _client.ExecuteAsync<FineTuneEventsResult>($"{_configuration.GetUri(OpenAi.FineTune, fineTuneId)}/{fineTuneId}/events", null, cancellationToken);
        public ValueTask<FineTuneDeleteResult> DeleteAsync(string fineTuneId, CancellationToken cancellationToken = default)
            => _client.DeleteAsync<FineTuneDeleteResult>($"{_configuration.GetUri(OpenAi.Model, fineTuneId)}/{fineTuneId}", null, cancellationToken);
    }
}
