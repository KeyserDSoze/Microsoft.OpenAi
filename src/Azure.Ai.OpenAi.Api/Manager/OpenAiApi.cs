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
