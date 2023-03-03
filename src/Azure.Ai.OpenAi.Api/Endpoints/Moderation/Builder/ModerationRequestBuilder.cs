using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Azure.Ai.OpenAi.Models;

namespace Azure.Ai.OpenAi
{
    public sealed class ModerationRequestBuilder : RequestBuilder<ModerationsRequest>
    {
        private static readonly List<Model> s_models = new List<Model>()
        {
            Model.TextModerationLatest,
            Model.TextModerationStable
        };
        public override List<Model> AvailableModels => s_models;

        internal ModerationRequestBuilder(HttpClient client, OpenAiConfiguration configuration, string input)
            : base(client, configuration, () =>
            {
                return new ModerationsRequest()
                {
                    Input = input,
                };
            })
        {
        }
        /// <summary>
        /// Classifies if text violates OpenAI's Content Policy.
        /// </summary>
        /// <returns>Builder</returns>
        public ValueTask<ModerationsResponse> ExecuteAsync(CancellationToken cancellationToken = default)
            => _client.ExecuteAsync<ModerationsResponse>(_configuration.ModerationUri, _request, cancellationToken);
        /// <summary>
        /// ID of the model to use.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Builder</returns>
        public ModerationRequestBuilder WithModel(ModelType model)
        {
            _request.ModelId = Model.FromModelType(model).Id;
            return this;
        }
        /// <summary>
        /// ID of the model to use. You can use <see cref="IOpenAiModelApi.AllAsync()"/> to see all of your available models, or use a standard model like <see cref="Model.TextModerationStable"/>.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Builder</returns>
        public ModerationRequestBuilder WithModel(string modelId)
        {
            _request.ModelId = modelId;
            return this;
        }
    }
}
