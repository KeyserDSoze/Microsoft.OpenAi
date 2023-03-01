namespace Azure.Ai.OpenAi
{
    public interface IOpenAiEmbeddingApi
    {
        EmbeddingRequestBuilder Request(string input);
    }
}
