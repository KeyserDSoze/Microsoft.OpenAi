using OpenSource.OpenAi.Audio;
using OpenSource.OpenAi.Chat;
using OpenSource.OpenAi.Completion;
using OpenSource.OpenAi.Edit;
using OpenSource.OpenAi.Embedding;
using OpenSource.OpenAi.File;
using OpenSource.OpenAi.FineTune;
using OpenSource.OpenAi.Image;
using OpenSource.OpenAi.Models;
using OpenSource.OpenAi.Moderation;

namespace OpenSource.OpenAi
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
