namespace Microsoft.OpenAi.Api
{
    internal sealed class OpenAiApi : IOpenAiApi
    {
        public IOpenAiCompletionApi Completion { get; }
        public IOpenAiEmbeddingApi Embedding { get; }
        public IOpenAiModelApi Model { get; }
        public IOpenAiFileApi File { get; }

        public OpenAiApi(IOpenAiCompletionApi openAiCompletionApi,
            IOpenAiEmbeddingApi openAiEmbeddingApi,
            IOpenAiModelApi openAiModelApi,
            IOpenAiFileApi openAiFileApi)
        {
            Completion = openAiCompletionApi;
            Embedding = openAiEmbeddingApi;
            Model = openAiModelApi;
            File = openAiFileApi;
        }
    }
}
