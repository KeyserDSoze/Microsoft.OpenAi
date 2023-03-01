using Azure.Ai.OpenAi.Models;

namespace Azure.Ai.OpenAi
{
    public interface IOpenAiModelApi
    {
        /// <summary>
        /// Get details about a particular Model from the API, specifically properties such as <see cref="Model.OwnedBy"/> and permissions.
        /// </summary>
        /// <param name="id">The id/name of the model to get more details about</param>
        /// <returns>Asynchronously returns the <see cref="Model"/> with all available properties</returns>
        ValueTask<Model> GetDetailsAsync(string id, CancellationToken cancellationToken = default);
        /// <summary>
        /// List all models via the API
        /// </summary>
        /// <returns>Asynchronously returns the list of all <see cref="Model"/>s</returns>
        Task<List<Model>> AllAsync(CancellationToken cancellationToken = default);
    }
}
