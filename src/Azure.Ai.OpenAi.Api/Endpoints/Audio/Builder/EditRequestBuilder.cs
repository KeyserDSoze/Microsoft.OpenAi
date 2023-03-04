using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Azure.Ai.OpenAi.Image;
using Azure.Ai.OpenAi.Models;

namespace Azure.Ai.OpenAi.Audio
{
    public sealed class AudioRequestBuilder : RequestBuilder<AudioTranscriptionRequest>
    {
        public override List<Model> AvailableModels => Model.Empty;
        internal AudioRequestBuilder(HttpClient client, OpenAiConfiguration configuration, string instruction) :
            base(client, configuration, () =>
            {
                return new AudioTranscriptionRequest
                {
                    Instruction = instruction
                };
            })
        {
        }
        internal const string ResponseFormatUrl = "url";
        internal const string ResponseFormatB64Json = "b64_json";
        /// <summary>
        /// Creates an image given a prompt.
        /// </summary>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>A list of generated texture urls to download.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public ValueTask<ImageResult> GetUrlAsync(CancellationToken cancellationToken = default)
        {
            _request.ResponseFormat = ResponseFormatUrl;
            var uri = $"{_configuration.ImageUri}/generations";
            return _client.ExecuteAsync<ImageResult>(uri, _request, cancellationToken);
        }
        /// <summary>
        /// Creates an image given a prompt.
        /// </summary>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>A list of generated texture urls to download.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async IAsyncEnumerable<Stream> DownloadAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var responses = await GetUrlAsync(cancellationToken);
            using var client = new HttpClient();
            foreach (var image in responses.Data)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var response = await client.GetAsync(image.Url);
                response.EnsureSuccessStatusCode();
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    using var stream = await response.Content.ReadAsStreamAsync();
                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    yield return memoryStream;
                }
            }
        }
        /// <summary>
        /// The number of images to generate. Must be between 1 and 10.
        /// </summary>
        /// <param name="numberOfResults"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public AudioRequestBuilder WithNumberOfResults(int numberOfResults)
        {
            if (numberOfResults > 10 || numberOfResults < 1)
                throw new ArgumentOutOfRangeException(nameof(numberOfResults), "The number of results must be between 1 and 10");
            _request.NumberOfResults = numberOfResults;
            return this;
        }
        /// <summary>
        /// The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public AudioRequestBuilder WithSize(ImageSize size)
        {
            _request.Size = size.AsString();
            return this;
        }
        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// <see href="https://platform.openai.com/docs/guides/safety-best-practices/end-user-ids"></see>
        /// </summary>
        /// <param name="user">Unique identifier</param>
        /// <returns>Builder</returns>
        public AudioRequestBuilder WithUser(string user)
        {
            _request.User = user;
            return this;
        }
    }
}
