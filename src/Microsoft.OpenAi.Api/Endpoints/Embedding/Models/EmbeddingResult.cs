using System.Text.Json.Serialization;

namespace Microsoft.OpenAi.Api.Embedding
{
    /// <summary>
    /// Represents an embedding result returned by the Embedding API.  
    /// </summary>
    public class EmbeddingResult : ApiResultBase
    {
        /// <summary>
        /// List of results of the embedding
        /// </summary>
        [JsonPropertyName("data")]
        public List<Data> Data { get; set; }
        /// <summary>
        /// Usage statistics of how many tokens have been used for this request
        /// </summary>
        [JsonPropertyName("usage")]
        public Usage Usage { get; set; }
        /// <summary>
        /// Allows an EmbeddingResult to be implicitly cast to the array of floats repsresenting the first ebmedding result
        /// </summary>
        /// <param name="embeddingResult">The <see cref="EmbeddingResult"/> to cast to an array of floats.</param>
        public static implicit operator float[](EmbeddingResult embeddingResult)
        {
            return embeddingResult.Data.FirstOrDefault()?.Embedding;
        }
    }
}
