using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Azure.Ai.OpenAi.Endpoints.Image.Models;

namespace Azure.Ai.OpenAi
{
    public sealed class ImageEditRequestBuilder
    {
        private readonly HttpClient _client;
        private readonly OpenAiConfiguration _configuration;
        private readonly ImageEditRequest _imageEditRequest;
        internal ImageEditRequestBuilder(HttpClient client, OpenAiConfiguration configuration, string prompt,
            Stream image, string imageName)
        {
            _client = client;
            _configuration = configuration;
            _imageEditRequest = new ImageEditRequest()
            {
                Prompt = prompt,
                NumberOfResults = 1,
                Size = "1024x1024",
            };
            var memoryStream = new MemoryStream();
            image.CopyTo(memoryStream);
            _imageEditRequest.Image = memoryStream;
            _imageEditRequest.ImageName = imageName;
        }
        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </summary>
        /// <param name="user">Unique identifier</param>
        /// <returns>Builder</returns>
        public ImageEditRequestBuilder WithUser(string user)
        {
            _imageEditRequest.User = user;
            return this;
        }
        public ImageEditRequestBuilder WithNumberOfResults(int numberOfResults)
        {
            if (numberOfResults > 10 || numberOfResults < 1)
                throw new ArgumentOutOfRangeException(nameof(numberOfResults), "The number of results must be between 1 and 10");
            _imageEditRequest.NumberOfResults = numberOfResults;
            return this;
        }
        public ImageEditRequestBuilder WithSize(ImageSize size)
        {
            switch (size)
            {
                case ImageSize.Small:
                    _imageEditRequest.Size = "256x256";
                    break;
                case ImageSize.Medium:
                    _imageEditRequest.Size = "512x512";
                    break;
                default:
                    _imageEditRequest.Size = "1024x1024";
                    break;
            }
            return this;
        }
        public ImageEditRequestBuilder WithMask(Stream mask, string maskName = "mask.png")
        {
            var memoryStream = new MemoryStream();
            mask.CopyTo(memoryStream);
            _imageEditRequest.Mask = memoryStream;
            _imageEditRequest.MaskName = maskName;
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
            await _imageEditRequest.Image.CopyToAsync(imageData, cancellationToken);
            imageData.Position = 0;
            content.Add(new ByteArrayContent(imageData.ToArray()), "image", _imageEditRequest.ImageName);

            if (_imageEditRequest.Mask != null)
            {
                using var maskData = new MemoryStream();
                await _imageEditRequest.Mask.CopyToAsync(maskData, cancellationToken);
                maskData.Position = 0;
                content.Add(new ByteArrayContent(maskData.ToArray()), "mask", _imageEditRequest.MaskName);
            }

            content.Add(new StringContent(_imageEditRequest.Prompt), "prompt");
            content.Add(new StringContent(_imageEditRequest.NumberOfResults.ToString()), "n");
            content.Add(new StringContent(_imageEditRequest.Size), "size");

            if (!string.IsNullOrWhiteSpace(_imageEditRequest.User))
            {
                content.Add(new StringContent(_imageEditRequest.User), "user");
            }
            _imageEditRequest.Dispose();

            var response = await _client.ExecuteAsync<ImagesResponse>($"{_configuration.ImageUri}/edits", content, cancellationToken);
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
            var responses = await _client.ExecuteAsync<ImagesResponse>(uri, _imageEditRequest, cancellationToken);
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
