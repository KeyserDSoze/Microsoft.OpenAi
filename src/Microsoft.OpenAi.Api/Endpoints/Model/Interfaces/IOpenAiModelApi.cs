using Microsoft.OpenAi.Api.Models;

namespace Microsoft.OpenAi.Api
{
    public interface IOpenAiModelApi
    {
        /// <summary>
        /// Get details about a particular Model from the API, specifically properties such as <see cref="Model.OwnedBy"/> and permissions.
        /// </summary>
        /// <param name="id">The id/name of the model to get more details about</param>
        /// <returns>Asynchronously returns the <see cref="Model"/> with all available properties</returns>
        ValueTask<Model> GetDetailsAsync(string id);
        /// <summary>
        /// List all models via the API
        /// </summary>
        /// <returns>Asynchronously returns the list of all <see cref="Model"/>s</returns>
        Task<List<Model>> AllAsync();
    }
}
