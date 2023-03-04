using Azure.Ai.OpenAi.Audio;
using Azure.Ai.OpenAi.Chat;
using Azure.Ai.OpenAi.Completion;
using Azure.Ai.OpenAi.Edit;
using Azure.Ai.OpenAi.Embedding;
using Azure.Ai.OpenAi.File;
using Azure.Ai.OpenAi.FineTune;
using Azure.Ai.OpenAi.Image;
using Azure.Ai.OpenAi.Models;
using Azure.Ai.OpenAi.Moderation;

namespace Azure.Ai.OpenAi
{
    public interface IOpenAiApiFactory
    {
        IOpenAiApi CreateApi();
    }
    public interface IOpenAiApi
    {
        IOpenAiModelApi Model { get; }
        IOpenAiFileApi File { get; }
        IOpenAiFineTuneApi FineTune { get; }
        IOpenAiChatApi Chat { get; }
        IOpenAiEditApi Edit { get; }
        IOpenAiCompletionApi Completion { get; }
        IOpenAiImageApi Image { get; }
        IOpenAiEmbeddingApi Embedding { get; }
        IOpenAiModerationApi Moderation { get; }
        IOpenAiAudioApi Audio { get; }
    }
}
