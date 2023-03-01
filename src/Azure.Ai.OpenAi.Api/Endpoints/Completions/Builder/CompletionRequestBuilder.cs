using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Azure.Ai.OpenAi.Completions;
using Azure.Ai.OpenAi.Models;

namespace Azure.Ai.OpenAi
{
    public sealed class CompletionRequestBuilder
    {
        private readonly HttpClient _client;
        private readonly OpenAiConfiguration _configuration;
        private readonly CompletionRequest _completionRequest;
        internal CompletionRequestBuilder(HttpClient client, OpenAiConfiguration configuration, string[] prompts)
        {
            _client = client;
            _configuration = configuration;
            _completionRequest = new CompletionRequest()
            {
                Prompt = prompts.Length > 1 ? (object)prompts : (prompts.Length == 1 ? prompts[1] : string.Empty),
                ModelId = Model.DefaultModel.Id,
            };
        }
        /// <summary>
        /// Specifies where the results should stream and be returned at one time.
        /// </summary>
        /// <returns>Builder</returns>
        public ValueTask<CompletionResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            _completionRequest.Stream = false;
            return _client.ExecuteAsync<CompletionResult>(_configuration.CompletionUri, _completionRequest, cancellationToken);
        }
        /// <summary>
        /// Specifies where the results should stream and be returned at one time.
        /// </summary>
        /// <returns>Builder</returns>
        public IAsyncEnumerable<CompletionResult> ExecuteAsStreamAsync(CancellationToken cancellationToken = default)
        {
            _completionRequest.Stream = true;
            _completionRequest.BestOf = null;
            return _client.ExecuteStreamAsync<CompletionResult>(_configuration.CompletionUri, _completionRequest, cancellationToken);
        }
        /// <summary>
        /// Add further prompt to the request.
        /// </summary>
        /// <param name="prompt">Prompt</param>
        /// <returns>Builder</returns>
        public CompletionRequestBuilder AddPrompt(string prompt)
        {
            if (_completionRequest.Prompt is string[] array)
            {
                var newArray = new string[array.Length + 1];
                array.CopyTo(newArray, 0);
                newArray[^1] = prompt;
                _completionRequest.Prompt = newArray;
            }
            else if (_completionRequest.Prompt is string value)
            {
                _completionRequest.Prompt = new string[2] { value, prompt };
            }
            else
            {
                _completionRequest.Prompt = prompt;
            }
            return this;
        }
        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </summary>
        /// <param name="user">Unique identifier</param>
        /// <returns>Builder</returns>
        public CompletionRequestBuilder WithUser(string user)
        {
            _completionRequest.User = user;
            return this;
        }
        /// <summary>
        /// Generates best_of completions server-side and returns the "best" (the one with the highest log probability per token). Results cannot be streamed.
        /// When used with n, best_of controls the number of candidate completions and n specifies how many to return – best_of must be greater than n.
        /// Note: Because this parameter generates many completions, it can quickly consume your token quota.Use carefully and ensure that you have reasonable settings for max_tokens and stop.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Builder</returns>
        public CompletionRequestBuilder BestOf(int value)
        {
            _completionRequest.Stream = false;
            _completionRequest.BestOf = value;
            return this;
        }
        /// <summary>
        /// One or more sequences where the API will stop generating further tokens. The returned text will not contain the stop sequence.
        /// </summary>
        /// <param name="values">Sequences</param>
        /// <returns>Builder</returns>
        public CompletionRequestBuilder WithStopSequence(params string[] values)
        {
            if (values.Length > 1)
                _completionRequest.StopSequence = values;
            else if (values.Length == 1)
                _completionRequest.StopSequence = values[0];
            return this;
        }
        /// <summary>
        /// Echo back the prompt in addition to the completion.
        /// </summary>
        /// <returns>Builder</returns>
        public CompletionRequestBuilder WithEcho()
        {
            _completionRequest.Echo = true;
            return this;
        }
        /// <summary>
        /// Include the log probabilities on the logprobs most likely tokens, which can be found in <see cref="CompletionResult.Completions"/> -> <see cref="Choice.Logprobs"/>. So for example, if logprobs is 5, the API will return a list of the 5 most likely tokens. If logprobs is supplied, the API will always return the logprob of the sampled token, so there may be up to logprobs+1 elements in the response. The maximum value for logprobs is 5.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Builder</returns>
        public CompletionRequestBuilder WithLogProbs(int value)
        {
            _completionRequest.Logprobs = value;
            return this;
        }

        /// <summary>
        /// How many different choices to request for each prompt.  Defaults to 1.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Builder</returns>
        public CompletionRequestBuilder WithNumberOfChoicesPerPrompt(int value)
        {
            _completionRequest.NumberOfChoicesPerPrompt = value;
            return this;
        }
        /// <summary>
        /// The scale of the penalty for how often a token is used.  Should generally be between 0 and 1, although negative numbers are allowed to encourage token reuse.  Defaults to 0.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Builder</returns>
        public CompletionRequestBuilder WithFrequencyPenalty(double value)
        {
            if (value < -1)
                throw new ArgumentException("Frequency penalty with a value lesser than -1");
            if (value > 1)
                throw new ArgumentException("Frequency penalty with a value greater than 1");
            _completionRequest.FrequencyPenalty = value;
            return this;
        }
        /// <summary>
        /// The scale of the penalty applied if a token is already present at all.  Should generally be between 0 and 1, although negative numbers are allowed to encourage token reuse.  Defaults to 0.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Builder</returns>
        public CompletionRequestBuilder WithPresencePenalty(double value)
        {
            if (value < -1)
                throw new ArgumentException("Presence penalty with a value lesser than -1");
            if (value > 1)
                throw new ArgumentException("Presence penalty with a value greater than 1");
            _completionRequest.PresencePenalty = value;
            return this;
        }
        /// <summary>
        /// An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered. It is generally recommend to use this or temperature but not both.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Builder</returns>
        public CompletionRequestBuilder WithNucleusSampling(double value)
        {
            if (value < 0)
                throw new ArgumentException("Nucleus sampling with a value lesser than 0");
            if (value > 1)
                throw new ArgumentException("Nucleus sampling with a value greater than 1");
            _completionRequest.TopP = value;
            return this;
        }
        /// <summary>
        /// What sampling temperature to use. Higher values means the model will take more risks. Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer. It is generally recommend to use this or Nuclues sampling (TopP) but not both.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Builder</returns>
        public CompletionRequestBuilder WithTemperature(double value)
        {
            if (value < 0)
                throw new ArgumentException("Temperature with a value lesser than 0");
            if (value > 1)
                throw new ArgumentException("Temperature with a value greater than 1");
            _completionRequest.Temperature = value;
            return this;
        }
        /// <summary>
        /// ID of the model to use.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Builder</returns>
        public CompletionRequestBuilder WithModel(ModelType model)
        {
            _completionRequest.ModelId = Model.FromModelType(model).Id;
            return this;
        }
        /// <summary>
        /// ID of the model to use. You can use <see cref="IOpenAiModelApi.AllAsync()"/> to see all of your available models, or use a standard model like <see cref="Model.DavinciText"/>.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Builder</returns>
        public CompletionRequestBuilder WithModel(string modelId)
        {
            _completionRequest.ModelId = modelId;
            return this;
        }
        /// <summary>
        /// How many tokens to complete to. Can return fewer if a stop sequence is hit.  Defaults to 16.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Builder</returns>
        public CompletionRequestBuilder SetMaxTokens(int value)
        {
            _completionRequest.MaxTokens = value;
            return this;
        }
        /// <summary>
        /// The suffix that comes after a completion of inserted text. Defaults to null.
        /// </summary>
        /// <param name="suffix">Suffix</param>
        /// <returns>Builder</returns>
        public CompletionRequestBuilder WithSuffix(string suffix)
        {
            _completionRequest.Suffix = suffix;
            return this;
        }
    }
}
