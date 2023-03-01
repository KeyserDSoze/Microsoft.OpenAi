using System.Text.Json.Serialization;

namespace Microsoft.OpenAi.Api.Files
{
    public partial class FilesEndpoint
    {
        /// <summary>
        /// A helper class to deserialize the JSON API responses.  This should not be used directly.
        /// </summary>
        private class FilesData : ApiResultBase
        {
            [JsonPropertyName("data")]
            public List<FileResult> Data { get; set; }
            [JsonPropertyName("object")]
            public string Obj { get; set; }
        }
    }


}
