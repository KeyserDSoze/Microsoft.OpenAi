using Azure.Ai.OpenAi.Embedding;
using Azure.Ai.OpenAi.Models;

namespace Azure.Ai.OpenAi
{
    public sealed class EmbeddingRequestBuilder
    {
        private readonly HttpClient _client;
        private readonly OpenAiConfiguration _configuration;
        private readonly EmbeddingRequest _embeddingRequest;
        internal EmbeddingRequestBuilder(HttpClient client, OpenAiConfiguration configuration, string input)
        {
            _client = client;
            _configuration = configuration;
            _embeddingRequest = new EmbeddingRequest()
            {
                Input = input,
                ModelId = Model.AdaTextEmbedding.Id,
            };
        }
        /// <summary>
        /// Specifies where the results should stream and be returned at one time.
        /// </summary>
        /// <returns>Builder</returns>
        public ValueTask<EmbeddingResult> ExecuteAsync(CancellationToken cancellationToken = default)
            => _client.ExecuteAsync<EmbeddingResult>(_configuration.EmbeddingUri, _embeddingRequest, cancellationToken);
        /// <summary>
        /// ID of the model to use.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Builder</returns>
        public EmbeddingRequestBuilder WithModel(ModelType model)
        {
            _embeddingRequest.ModelId = Model.FromModelType(model).Id;
            return this;
        }
        /// <summary>
        /// ID of the model to use. You can use <see cref="IOpenAiModelApi.AllAsync()"/> to see all of your available models, or use a standard model like <see cref="Model.DavinciText"/>.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Builder</returns>
        public EmbeddingRequestBuilder WithModel(string modelId)
        {
            _embeddingRequest.ModelId = modelId;
            return this;
        }
    }
}
