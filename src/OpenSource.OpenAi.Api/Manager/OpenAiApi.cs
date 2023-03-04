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
    internal sealed class OpenAiApi : IOpenAiApi
    {
        public IOpenAiModelApi Model { get; }
        public IOpenAiCompletionApi Completion { get; }
        public IOpenAiImageApi Image { get; }
        public IOpenAiEmbeddingApi Embedding { get; }
        public IOpenAiFileApi File { get; }
        public IOpenAiModerationApi Moderation { get; }
        public IOpenAiAudioApi Audio { get; }
        public IOpenAiFineTuneApi FineTune { get; }
        public IOpenAiChatApi Chat { get; }
        public IOpenAiEditApi Edit { get; }

        public OpenAiApi(IOpenAiCompletionApi completionApi,
            IOpenAiEmbeddingApi embeddingApi,
            IOpenAiModelApi modelApi,
            IOpenAiFileApi fileApi,
            IOpenAiImageApi imageApi,
            IOpenAiModerationApi moderationApi,
            IOpenAiAudioApi audioApi,
            IOpenAiFineTuneApi fineTuneApi,
            IOpenAiChatApi chatApi,
            IOpenAiEditApi editApi)
        {
            Completion = completionApi;
            Embedding = embeddingApi;
            Model = modelApi;
            File = fileApi;
            Image = imageApi;
            Moderation = moderationApi;
            Audio = audioApi;
            FineTune = fineTuneApi;
            Chat = chatApi;
            Edit = editApi;
        }
    }
}
