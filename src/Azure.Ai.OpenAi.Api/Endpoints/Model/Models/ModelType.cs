namespace Azure.Ai.OpenAi.Models
{
    public enum AudioModelType
    {
        /// <summary>
        /// Whisper is a general-purpose speech recognition model. It is trained on a large dataset of diverse audio and is also a multi-task model that can perform multilingual speech recognition as well as speech translation and language identification. The Whisper v2-large model is currently available through our API with the whisper-1 model name.
        /// </summary>
        Whisper = 0,
    }
    public static class AudioModelTypeExtensions
    {
        private static readonly Model s_whisper = new Model("whisper-1");
        public static Model ToModel(this AudioModelType type)
        {
            switch (type)
            {
                default:
                case AudioModelType.Whisper:
                    return s_whisper;
            }
        }
    }
    public enum TextModelType
    {
        /// <summary>
        /// Ada is usually the fastest model and can perform tasks like parsing text, address correction and certain kinds of classification tasks that don’t require too much nuance. Ada’s performance can often be improved by providing more context.
        /// <b>Good at: Parsing text, simple classification, address correction, keywords</b>
        /// <i>Note: Any task performed by a faster model like Ada can be performed by a more powerful model like Curie or Davinci.</i>
        /// </summary>
        AdaText = 0,
        /// <summary>
        /// Babbage can perform straightforward tasks like simple classification. It’s also quite capable when it comes to Semantic Search ranking how well documents match up with search queries.
        /// <b>Good at: Moderate classification, semantic search classification</b>
        /// </summary>
        BabbageText = 100,
        /// <summary>
        /// Curie is extremely powerful, yet very fast. While Davinci is stronger when it comes to analyzing complicated text, Curie is quite capable for many nuanced tasks like sentiment classification and summarization. Curie is also quite good at answering questions and performing Q&A and as a general service chatbot.
        /// <b>Good at: Language translation, complex classification, text sentiment, summarization</b>
        /// </summary>
        CurieText = 200,
        /// <summary>
        /// Similar capabilities to text-davinci-003 but trained with supervised fine-tuning instead of reinforcement learning. <b>(4000 tokens) Up to Jun 2021</b>.
        /// Davinci is the most capable model family and can perform any task the other models (ada, curie, and babbage) can perform and often with less instruction. For applications requiring a lot of understanding of the content, like summarization for a specific audience and creative content generation, Davinci will produce the best results. These increased capabilities require more compute resources, so Davinci costs more per API call and is not as fast as the other models
        /// Another area where Davinci shines is in understanding the intent of text. Davinci is quite good at solving many kinds of logic problems and explaining the motives of characters. Davinci has been able to solve some of the most challenging AI problems involving cause and effect.
        /// <b>Good at: Complex intent, cause and effect, summarization for audience</b>
        /// </summary>
        DavinciText2 = 301,
        /// <summary>
        /// Can do any language task with better quality, longer output, and consistent instruction-following than the curie, babbage, or ada models. Also supports <see href="https://platform.openai.com/docs/guides/completion/inserting-text">inserting</see> completions within text. <b>(4000 tokens) Up to Jun 2021</b>.
        /// Davinci is the most capable model family and can perform any task the other models (ada, curie, and babbage) can perform and often with less instruction. For applications requiring a lot of understanding of the content, like summarization for a specific audience and creative content generation, Davinci will produce the best results. These increased capabilities require more compute resources, so Davinci costs more per API call and is not as fast as the other models
        /// Another area where Davinci shines is in understanding the intent of text. Davinci is quite good at solving many kinds of logic problems and explaining the motives of characters. Davinci has been able to solve some of the most challenging AI problems involving cause and effect.
        /// <b>Good at: Complex intent, cause and effect, summarization for audience</b>
        /// </summary>
        DavinciText3 = 302,
        /// <summary>
        /// Optimized for code-completion tasks <b>(4000 tokens) Up to Jun 2021</b>.
        /// Most capable Codex model. Particularly good at translating natural language to code. In addition to completing code, also supports <see href="https://platform.openai.com/docs/guides/code/inserting-code">inserting</see> completions within code.
        /// </summary>
        DavinciCode = 400,
        /// <summary>
        /// Almost as capable as Davinci Codex, but slightly faster. This speed advantage may make it preferable for real-time applications.
        /// <b>Up to 2,048 tokens</b>
        /// </summary>
        CushmanCode = 500,
    }
    public static class TextModelTypeExtensions
    {
        private static readonly Model s_adaText = new Model("text-ada-001");
        private static readonly Model s_babbageText = new Model("text-babbage-001");
        private static readonly Model s_curieText = new Model("text-curie-001");
        private static readonly Model s_davinciText3 = new Model("text-davinci-003");
        private static readonly Model s_davinciText2 = new Model("text-davinci-002");
        private static readonly Model s_davinciCode = new Model("code-davinci-002");
        private static readonly Model s_cushmanCode = new Model("code-cushman-001");
        public static Model ToModel(this TextModelType type)
        {
            switch (type)
            {
                case TextModelType.BabbageText:
                    return s_babbageText;
                case TextModelType.CurieText:
                    return s_curieText;
                case TextModelType.DavinciText3:
                    return s_davinciText3;
                case TextModelType.DavinciText2:
                    return s_davinciText2;
                case TextModelType.DavinciCode:
                    return s_davinciCode;
                case TextModelType.CushmanCode:
                    return s_cushmanCode;
                default:
                case TextModelType.AdaText:
                    return s_adaText;
            }
        }
    }
    /// <summary>
    /// Embeddings are a numerical representation of text that can be used to measure the relateness between two pieces of text. Our second generation embedding model, text-embedding-ada-002 is a designed to replace the previous 16 first-generation embedding models at a fraction of the cost. Embeddings are useful for search, clustering, recommendations, anomaly detection, and classification tasks. You can read more about our latest embedding model in the <see href="https://openai.com/blog/new-and-improved-embedding-model">announcement blog post</see>.
    /// </summary>
    public enum EmbeddingModelType
    {
        AdaTextEmbedding,
    }
    public static class EmbeddingModelTypeExtensions
    {
        private static readonly Model s_adaTextEmbedding = new Model("text-embedding-ada-002");
        public static Model ToModel(this EmbeddingModelType type)
        {
            switch (type)
            {
                default:
                case EmbeddingModelType.AdaTextEmbedding:
                    return s_adaTextEmbedding;
            }
        }
    }
    /// <summary>
    /// The Moderation models are designed to check whether content complies with <see href="https://platform.openai.com/docs/usage-policies/">OpenAI's usage policies</see>. The models provide classification capabilities that look for content in the following categories: hate, hate/threatening, self-harm, sexual, sexual/minors, violence, and violence/graphic. You can find out more in our <see href="https://platform.openai.com/docs/guides/moderation/overview">moderation guide</see>.
    /// </summary>
    public enum ModerationModelType
    {
        /// <summary>
        /// Almost as capable as the latest model, but slightly older.
        /// </summary>
        TextModerationStable = 0,
        /// <summary>
        /// Most capable moderation model. Accuracy will be slighlty higher than the stable model
        /// </summary>
        TextModerationLatest = 100,
    }
    public static class ModerationModelTypeExtensions
    {
        private static readonly Model s_textModerationStable = new Model("text-moderation-stable");
        private static readonly Model s_textModerationLatest = new Model("text-moderation-latest");
        public static Model ToModel(this ModerationModelType type)
        {
            switch (type)
            {
                case ModerationModelType.TextModerationStable:
                    return s_textModerationStable;
                default:
                case ModerationModelType.TextModerationLatest:
                    return s_textModerationLatest;
            }
        }
    }
    public enum ChatModelType
    {
        /// <summary>
        /// Turbo is the same model family that powers ChatGPT. It is optimized for conversational chat input and output but does equally well on completions when compared with the Davinci model family. Any use case that can be done well in ChatGPT should perform well with the Turbo model family in the API.
        /// </summary>
        Gpt35Turbo = 0,
        /// <summary>
        /// Turbo is the same model family that powers ChatGPT. It is optimized for conversational chat input and output but does equally well on completions when compared with the Davinci model family. Any use case that can be done well in ChatGPT should perform well with the Turbo model family in the API.
        /// </summary>
        Gpt35Turbo0301 = 1,
    }
    public static class ChatModelTypeExtensions
    {
        private static readonly Model s_gpt35Turbo = new Model("gpt-3.5-turbo");
        private static readonly Model s_gpt35Turbo0301 = new Model("gpt-3.5-turbo-0301");
        public static Model ToModel(this ChatModelType type)
        {
            switch (type)
            {
                case ChatModelType.Gpt35Turbo:
                    return s_gpt35Turbo;
                default:
                case ChatModelType.Gpt35Turbo0301:
                    return s_gpt35Turbo0301;
            }
        }
    }
    public enum EditModelType
    {
        /// <summary>
        /// Optimized for text-edit tasks
        /// </summary>
        TextDavinciEdit,
        /// <summary>
        /// Optimized for code-edit tasks
        /// </summary>
        CodeDavinciEdit = 100,
    }
    public static class EditModelTypeExtensions
    {
        private static readonly Model s_textDavinciEdit = new Model("text-davinci-edit-001");
        private static readonly Model s_codeDavinciEdit = new Model("code-davinci-edit-001");
        public static Model ToModel(this EditModelType type)
        {
            switch (type)
            {
                case EditModelType.TextDavinciEdit:
                    return s_textDavinciEdit;
                default:
                case EditModelType.CodeDavinciEdit:
                    return s_codeDavinciEdit;
            }
        }
    }
}
