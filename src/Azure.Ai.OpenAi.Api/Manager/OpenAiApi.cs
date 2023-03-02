namespace Azure.Ai.OpenAi
{
    internal sealed class OpenAiApi : IOpenAiApi
    {
        public IOpenAiModelApi Model { get; }
        public IOpenAiCompletionApi Completion { get; }
        public IOpenAiImageApi Image { get; }
        public IOpenAiEmbeddingApi Embedding { get; }
        public IOpenAiFileApi File { get; }
        public IOpenAiModerationApi Moderation { get; }

        public OpenAiApi(IOpenAiCompletionApi openAiCompletionApi,
            IOpenAiEmbeddingApi openAiEmbeddingApi,
            IOpenAiModelApi openAiModelApi,
            IOpenAiFileApi openAiFileApi,
            IOpenAiImageApi image,
            IOpenAiModerationApi moderation)
        {
            Completion = openAiCompletionApi;
            Embedding = openAiEmbeddingApi;
            Model = openAiModelApi;
            File = openAiFileApi;
            Image = image;
            Moderation = moderation;
        }
    }
}
