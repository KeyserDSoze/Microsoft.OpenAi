﻿using System.Text.Json.Serialization;
using Microsoft.OpenAi.Api.Files;

namespace Microsoft.OpenAi.Api
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
        /// <summary>
        /// Get the list of all files
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<List<FileResult>> AllAsync()
        {
            var response = await _client.ExecuteAsync<FilesDataHelper>(_configuration.FileUri, null);
            return response.Data;
        }
        /// <summary>
        /// Returns information about a specific file
        /// </summary>
        /// <param name="fileId">The ID of the file to use for this request</param>
        /// <returns></returns>
        public ValueTask<FileResult> GetAsync(string fileId)
            => _client.ExecuteAsync<FileResult>($"{_configuration.FileUri}/{fileId}", null);
        /// <summary>
        /// Returns the contents of the specific file as string
        /// </summary>
        /// <param name="fileId">The ID of the file to use for this request</param>
        /// <returns></returns>
        public async Task<string> GetFileContentAsStringAsync(string fileId)
        {
            var response = await _client.PrivatedExecuteAsync($"{_configuration.FileUri}/{fileId}/content", null, false, false);
            return await response.Content.ReadAsStringAsync();
        }
        /// <summary>
        /// Delete a file
        ///	</summary>
        ///	 <param name="fileId">The ID of the file to use for this request</param>
        /// <returns></returns>
        public ValueTask<FileResult> DeleteAsync(string fileId)
            => _client.DeleteAsync<FileResult>($"{_configuration.FileUri}/{fileId}", null);
        private const string Purpose = "purpose";
        private const string FileContent = "file";
        /// <summary>
        /// Upload a file that contains document(s) to be used across various endpoints/features. Currently, the size of all the files uploaded by one organization can be up to 1 GB. Please contact OpenAI if you need to increase the storage limit.
        /// </summary>
        /// <param name="file">The stream for the file to use for this request</param>
        /// <param name="purpose">The intendend purpose of the uploaded documents. Use "fine-tune" for Fine-tuning. This allows us to validate the format of the uploaded file.</param>
        public ValueTask<FileResult> UploadFileAsync(Stream file, string fileName, string purpose = "fine-tune")
        {
            var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            var content = new MultipartFormDataContent
            {
                { new StringContent(purpose), Purpose },
                { new ByteArrayContent(memoryStream.ToArray()), FileContent, fileName }
            };
            return _client.ExecuteAsync<FileResult>(_configuration.FileUri, content);
        }
        private sealed class FilesDataHelper : ApiResultBase
        {
            [JsonPropertyName("data")]
            public List<FileResult> Data { get; set; }
            [JsonPropertyName("object")]
            public string Obj { get; set; }
        }
    }
}
