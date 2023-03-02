namespace Azure.Ai.OpenAi
{
    public interface IOpenAiApi
    {
        IOpenAiModelApi Model { get; }
        IOpenAiCompletionApi Completion { get; }
        IOpenAiImageApi Image { get; }
        IOpenAiEmbeddingApi Embedding { get; }
        IOpenAiFileApi File { get; }
        IOpenAiModerationApi Moderation { get; }
    }
}
