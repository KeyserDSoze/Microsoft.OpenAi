using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Azure.Ai.OpenAi.Models;

namespace Azure.Ai.OpenAi
{
    public abstract class RequestBuilder<T>
        where T : class, IOpenAiRequest
    {
        private protected readonly HttpClient _client;
        private protected readonly OpenAiConfiguration _configuration;
        private protected readonly T _request;
        public abstract List<Model> AvailableModels { get; }
        private protected Model DefaultModel => AvailableModels.FirstOrDefault();
        private protected RequestBuilder(HttpClient client, OpenAiConfiguration configuration, Func<T> requestCreator)
        {
            _client = client;
            _configuration = configuration;
            _request = requestCreator.Invoke();
            if (_request.ModelId == null)
                _request.ModelId = DefaultModel?.Id ?? string.Empty;
        }
    }
}
