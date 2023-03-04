using System.Text.Json.Serialization;

namespace OpenSource.OpenAi
{
    public interface IOpenAiRequest
    {
        string? ModelId { get; set; }
    }
}
