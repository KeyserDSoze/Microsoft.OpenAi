using System;
using System.Drawing;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Azure.Ai.OpenAi.Completions;
using Azure.Ai.OpenAi.Models;
using Microsoft.OpenAi.Api.Endpoints.Image.Models;

namespace Azure.Ai.OpenAi
{
    public sealed class ImageGenerationRequestBuilder
    {
        private readonly HttpClient _client;
        private readonly OpenAiConfiguration _configuration;
        private readonly ImageGenerationRequest _imageGenerationRequest;
        internal ImageGenerationRequestBuilder(HttpClient client, OpenAiConfiguration configuration, string prompt)
        {
            _client = client;
            _configuration = configuration;
            _imageGenerationRequest = new ImageGenerationRequest()
            {
                Prompt = prompt,
                NumberOfResults = 1,
                Size = "1024x1024",
            };
        }
        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </summary>
        /// <param name="user">Unique identifier</param>
        /// <returns>Builder</returns>
        public ImageGenerationRequestBuilder WithUser(string user)
        {
            _imageGenerationRequest.User = user;
            return this;
        }
        public ImageGenerationRequestBuilder WithNumberOfResults(int numberOfResults)
        {
            if (numberOfResults > 10 || numberOfResults < 1)
                throw new ArgumentOutOfRangeException(nameof(numberOfResults), "The number of results must be between 1 and 10");
            _imageGenerationRequest.NumberOfResults = numberOfResults;
            return this;
        }
        public ImageGenerationRequestBuilder WithSize(ImageSize size)
        {
            switch (size)
            {
                case ImageSize.Small:
                    _imageGenerationRequest.Size = "256x256";
                    break;
                case ImageSize.Medium:
                    _imageGenerationRequest.Size = "512x512";
                    break;
                default:
                    _imageGenerationRequest.Size = "1024x1024";
                    break;
            };
            return this;
        }
        /// <summary>
        /// Creates an image given a prompt.
        /// </summary>
        /// <param name="request"><see cref="ImageGenerationRequest"/></param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>A list of generated texture urls to download.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public ValueTask<ImagesResponse> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var uri = $"{_configuration.ImageUri}/generations";
            return _client.ExecuteAsync<ImagesResponse>(uri, _imageGenerationRequest, cancellationToken);
        }
        /// <summary>
        /// Creates an image given a prompt.
        /// </summary>
        /// <param name="request"><see cref="ImageGenerationRequest"/></param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>A list of generated texture urls to download.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async IAsyncEnumerable<Stream> DownloadAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var responses = await ExecuteAsync(cancellationToken);
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
        /// The image to use as the basis for the variation(s). Must be a valid PNG file, less than 4MB, and square.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageName"></param>
        /// <returns>Edit Builder</returns>
        public ImageEditRequestBuilder Edit(Stream image, string imageName = "image.png")
            => new ImageEditRequestBuilder(_client, _configuration, _imageGenerationRequest.Prompt, image, imageName);
    }
}
