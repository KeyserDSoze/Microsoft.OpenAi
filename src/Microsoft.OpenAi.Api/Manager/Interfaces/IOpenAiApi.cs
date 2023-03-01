namespace Microsoft.OpenAi.Api
{
    public interface IOpenAiApi
    {
        IOpenAiCompletionApi Completion { get; }
        IOpenAiEmbeddingApi Embedding { get; }
        IOpenAiModelApi Model { get; }
        IOpenAiFileApi File { get; }
    }
}
