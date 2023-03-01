using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Azure.Ai.OpenAi.Models;
using Azure.Ai.OpenAi.Endpoints.Image.Models;
using System.IO;
using System.Net.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Azure.Ai.OpenAi
{
    public sealed class ImageVariationRequestBuilder
    {
        private readonly HttpClient _client;
        private readonly OpenAiConfiguration _configuration;
        private readonly ImageVariationRequest _imageVariationRequest;
        internal ImageVariationRequestBuilder(HttpClient client, OpenAiConfiguration configuration, Stream image, string imageName)
        {
            _client = client;
            _configuration = configuration;
            _imageVariationRequest = new ImageVariationRequest()
            {
                NumberOfResults = 1,
                Size = "1024x1024",
            };
            var memoryStream = new MemoryStream();
            image.CopyTo(memoryStream);
            _imageVariationRequest.Image = memoryStream;
            _imageVariationRequest.ImageName = imageName;
        }
        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </summary>
        /// <param name="user">Unique identifier</param>
        /// <returns>Builder</returns>
        public ImageVariationRequestBuilder WithUser(string user)
        {
            _imageVariationRequest.User = user;
            return this;
        }
        public ImageVariationRequestBuilder WithNumberOfResults(int numberOfResults)
        {
            if (numberOfResults > 10 || numberOfResults < 1)
                throw new ArgumentOutOfRangeException(nameof(numberOfResults), "The number of results must be between 1 and 10");
            _imageVariationRequest.NumberOfResults = numberOfResults;
            return this;
        }
        public ImageVariationRequestBuilder WithSize(ImageSize size)
        {
            switch (size)
            {
                case ImageSize.Small:
                    _imageVariationRequest.Size = "256x256";
                    break;
                case ImageSize.Medium:
                    _imageVariationRequest.Size = "512x512";
                    break;
                default:
                    _imageVariationRequest.Size = "1024x1024";
                    break;
            }
            return this;
        }
        /// <summary>
        /// Creates an image given a prompt.
        /// </summary>
        /// <param name="request"><see cref="ImageGenerationRequest"/></param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>A list of generated texture urls to download.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async ValueTask<ImagesResponse> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            using var content = new MultipartFormDataContent();
            using var imageData = new MemoryStream();
            await _imageVariationRequest.Image.CopyToAsync(imageData, cancellationToken);
            imageData.Position = 0;
            content.Add(new ByteArrayContent(imageData.ToArray()), "image", _imageVariationRequest.ImageName);

            content.Add(new StringContent(_imageVariationRequest.NumberOfResults.ToString()), "n");
            content.Add(new StringContent(_imageVariationRequest.Size), "size");

            if (!string.IsNullOrWhiteSpace(_imageVariationRequest.User))
            {
                content.Add(new StringContent(_imageVariationRequest.User), "user");
            }
            _imageVariationRequest.Dispose();

            var response = await _client.ExecuteAsync<ImagesResponse>($"{_configuration.ImageUri}/variations", content, cancellationToken);
            return response;
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
            var uri = $"{_configuration.ImageUri}/generations";
            var responses = await _client.ExecuteAsync<ImagesResponse>(uri, _imageVariationRequest, cancellationToken);
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
    }
}
