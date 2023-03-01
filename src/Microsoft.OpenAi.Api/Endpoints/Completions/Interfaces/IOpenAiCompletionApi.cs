namespace Microsoft.OpenAi.Api
{
    public interface IOpenAiCompletionApi
    {
        CompletionRequestBuilder Request(params string[] prompts);
    }
}
