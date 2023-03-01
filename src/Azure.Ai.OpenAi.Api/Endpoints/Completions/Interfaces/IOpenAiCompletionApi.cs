namespace Azure.Ai.OpenAi
{
    public interface IOpenAiCompletionApi
    {
        CompletionRequestBuilder Request(params string[] prompts);
    }
}
