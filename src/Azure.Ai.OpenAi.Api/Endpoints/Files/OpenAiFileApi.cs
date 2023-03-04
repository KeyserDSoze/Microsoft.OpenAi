﻿using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Ai.OpenAi.File
{
    internal sealed class OpenAiFileApi : IOpenAiFileApi
    {
        private readonly HttpClient _client;
        private readonly OpenAiConfiguration _configuration;
        public OpenAiFileApi(IHttpClientFactory httpClientFactory, OpenAiConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient(OpenAiSettings.HttpClientName);
            _configuration = configuration;
        }

        public async Task<List<FileResult>> AllAsync(CancellationToken cancellationToken = default)
        {
            var response = await _client.ExecuteAsync<FilesData>(_configuration.FileUri, null, cancellationToken);
            return response.Data ?? new List<FileResult>();
        }
        private const string Purpose = "purpose";
        private const string FileContent = "file";
        public ValueTask<FileResult> UploadFileAsync(Stream file, string fileName, string purpose = "fine-tune", CancellationToken cancellationToken = default)
        {
            var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            var content = new MultipartFormDataContent
            {
                { new StringContent(purpose), Purpose },
                { new ByteArrayContent(memoryStream.ToArray()), FileContent, fileName }
            };
            return _client.ExecuteAsync<FileResult>(_configuration.FileUri, content, cancellationToken);
        }
        public ValueTask<FileResult> DeleteAsync(string fileId, CancellationToken cancellationToken = default)
            => _client.DeleteAsync<FileResult>($"{_configuration.FileUri}/{fileId}", null, cancellationToken);
        public ValueTask<FileResult> RetrieveAsync(string fileId, CancellationToken cancellationToken = default)
            => _client.ExecuteAsync<FileResult>($"{_configuration.FileUri}/{fileId}", null, cancellationToken);
        public async Task<string> RetrieveFileContentAsStringAsync(string fileId, CancellationToken cancellationToken = default)
        {
            var response = await _client.PrivatedExecuteAsync($"{_configuration.FileUri}/{fileId}/content", null, false, false, false, cancellationToken);
            return await response.Content.ReadAsStringAsync();
        }
        private sealed class FilesData : ApiBaseResponse
        {
            [JsonPropertyName("data")]
            public List<FileResult>? Data { get; set; }
        }
    }
}
