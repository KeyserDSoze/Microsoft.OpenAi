namespace Microsoft.OpenAi.Api
{
    public interface IOpenAiEmbeddingApi
    {
        EmbeddingRequestBuilder Request(string input);
    }
}
