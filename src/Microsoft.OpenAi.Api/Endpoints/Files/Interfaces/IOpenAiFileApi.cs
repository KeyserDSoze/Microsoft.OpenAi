using Microsoft.OpenAi.Api.Files;

namespace Microsoft.OpenAi.Api
{
    public interface IOpenAiFileApi
    {
        /// <summary>
        /// Get the list of all files
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        Task<List<FileResult>> AllAsync();
        /// <summary>
        /// Returns information about a specific file
        /// </summary>
        /// <param name="fileId">The ID of the file to use for this request</param>
        /// <returns></returns>
        ValueTask<FileResult> GetAsync(string fileId);
        /// <summary>
        /// Returns the contents of the specific file as string
        /// </summary>
        /// <param name="fileId">The ID of the file to use for this request</param>
        /// <returns></returns>
        Task<string> GetFileContentAsStringAsync(string fileId);
        /// <summary>
        /// Delete a file
        ///	</summary>
        ///	 <param name="fileId">The ID of the file to use for this request</param>
        /// <returns></returns>
        ValueTask<FileResult> DeleteAsync(string fileId);
        /// <summary>
        /// Upload a file that contains document(s) to be used across various endpoints/features. Currently, the size of all the files uploaded by one organization can be up to 1 GB. Please contact OpenAI if you need to increase the storage limit.
        /// </summary>
        /// <param name="file">The stream for the file to use for this request</param>
        /// <param name="purpose">The intendend purpose of the uploaded documents. Use "fine-tune" for Fine-tuning. This allows us to validate the format of the uploaded file.</param>
        ValueTask<FileResult> UploadFileAsync(Stream file, string fileName, string purpose = "fine-tune");
    }
}
