using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Azure.Ai.OpenAi.Models;

namespace Azure.Ai.OpenAi
{
    public sealed class ModerationRequestBuilder
    {
        private readonly HttpClient _client;
        private readonly OpenAiConfiguration _configuration;
        private readonly ModerationsRequest _moderationRequest;
        internal ModerationRequestBuilder(HttpClient client, OpenAiConfiguration configuration, string input)
        {
            _client = client;
            _configuration = configuration;
            _moderationRequest = new ModerationsRequest()
            {
                Input = input,
                ModelId = Model.TextModerationLatest.Id,
            };
        }
        /// <summary>
        /// Classifies if text violates OpenAI's Content Policy.
        /// </summary>
        /// <returns>Builder</returns>
        public ValueTask<ModerationsResponse> ExecuteAsync(CancellationToken cancellationToken = default)
            => _client.ExecuteAsync<ModerationsResponse>(_configuration.ModerationUri, _moderationRequest, cancellationToken);
        /// <summary>
        /// ID of the model to use.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Builder</returns>
        public ModerationRequestBuilder WithModel(ModelType model)
        {
            _moderationRequest.ModelId = Model.FromModelType(model).Id;
            return this;
        }
        /// <summary>
        /// ID of the model to use. You can use <see cref="IOpenAiModelApi.AllAsync()"/> to see all of your available models, or use a standard model like <see cref="Model.TextModerationStable"/>.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Builder</returns>
        public ModerationRequestBuilder WithModel(string modelId)
        {
            _moderationRequest.ModelId = modelId;
            return this;
        }
    }
}
