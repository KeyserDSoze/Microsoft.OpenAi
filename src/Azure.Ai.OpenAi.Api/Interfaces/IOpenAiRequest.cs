using System.Text.Json.Serialization;

namespace Azure.Ai.OpenAi
{
    public interface IOpenAiRequest
    {
        string? ModelId { get; set; }
    }
}
