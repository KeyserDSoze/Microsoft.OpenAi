using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Azure.Ai.OpenAi.Models
{
    /// <summary>
    /// Represents a language model
    /// </summary>
    public class Model
    {
        /// <summary>
        /// The id/name of the model
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
        /// <summary>
        /// The owner of this model.  Generally "openai" is a generic OpenAI model, or the organization if a custom or finetuned model.
        /// </summary>
        [JsonPropertyName("owned_by")]
        public string? OwnedBy { get; set; }
        /// <summary>
        /// The type of object. Should always be 'model'.
        /// </summary>
        [JsonPropertyName("object")]
        public string? Object { get; set; }
        [JsonIgnore]
        public DateTime? Created => CreatedUnixTime.HasValue ? (DateTime?)(DateTimeOffset.FromUnixTimeSeconds(CreatedUnixTime.Value).DateTime) : null;
        /// <summary>
        /// The time when the model was created in unix epoch format
        /// </summary>
        [JsonPropertyName("created")]
        public long? CreatedUnixTime { get; set; }
        /// <summary>
        /// Permissions for use of the model
        /// </summary>
        [JsonPropertyName("permission")]
        public List<Permissions> Permission { get; set; } = new List<Permissions>();
        /// <summary>
        /// Currently (2023-01-27) seems like this is duplicate of <see cref="Id"/> but including for completeness.
        /// </summary>
        [JsonPropertyName("root")]
        public string? Root { get; set; }
        /// <summary>
        /// Currently (2023-01-27) seems unused, probably intended for nesting of models in a later release
        /// </summary>
        [JsonPropertyName("parent")]
        public string? Parent { get; set; }
        /// <summary>
        /// Represents an Model with the given id/<see cref="Id"/>
        /// </summary>
        /// <param name="name">The id/<see cref="Id"/> to use.
        ///	</param>
        public Model(string name)
        {
            Id = name;
            OwnedBy = OwnedByOpenAi;
        }
        private const string OwnedByOpenAi = "openai";
        /// <summary>
        /// The default model to use in requests if no other model is specified.
        /// </summary>
        public static Model DefaultModel => DavinciText;
        /// <summary>
        /// Capable of very simple tasks, usually the fastest model in the GPT-3 series, and lowest cost
        /// </summary>
        public static Model AdaText { get; } = new Model("text-ada-001");
        /// <summary>
        /// Capable of straightforward tasks, very fast, and lower cost.
        /// </summary>
        public static Model BabbageText { get; } = new Model("text-babbage-001");
        /// <summary>
        /// Very capable, but faster and lower cost than Davinci.
        /// </summary>
        public static Model CurieText { get; } = new Model("text-curie-001");
        /// <summary>
        /// Most capable GPT-3 model. Can do any task the other models can do, often with higher quality, longer output and better instruction-following. Also supports inserting completions within text.
        /// </summary>
        public static Model DavinciText { get; } = new Model("text-davinci-003");
        /// <summary>
        /// Almost as capable as Davinci Codex, but slightly faster. This speed advantage may make it preferable for real-time applications.
        /// </summary>
        public static Model CushmanCode { get; } = new Model("code-cushman-001");
        /// <summary>
        /// Most capable Codex model. Particularly good at translating natural language to code. In addition to completing code, also supports inserting completions within code.
        /// </summary>
        public static Model DavinciCode { get; } = new Model("code-davinci-002");
        /// <summary>
        /// OpenAI offers one second-generation embedding model for use with the embeddings API endpoint.
        /// </summary>
        public static Model AdaTextEmbedding { get; } = new Model("text-embedding-ada-002");
        public static Model TextModerationStable { get; } = new Model("text-moderation-stable");
        public static Model TextModerationLatest { get; } = new Model("text-moderation-latest");
        public static Model Gpt35Turbo { get; } = new Model("gpt-3.5-turbo");
        public static Model Gpt35Turbo0301 { get; } = new Model("gpt-3.5-turbo-0301");
        public static Model TextDavinciEdit { get; } = new Model("text-davinci-edit-001");
        public static Model CodeDavinciEdit { get; } = new Model("code-davinci-edit-001");
        public static List<Model> All { get; } = new List<Model>()
        {
            AdaText,
            BabbageText,
            CurieText,
            DavinciText,
            CushmanCode,
            DavinciCode,
            AdaTextEmbedding,
            TextModerationStable,
            TextModerationLatest,
            Gpt35Turbo,
            Gpt35Turbo0301
        };
        internal static Model FromModelType(ModelType type)
        {
            switch (type)
            {
                case ModelType.Default:
                    return DefaultModel;
                case ModelType.DavinciText:
                    return DavinciText;
                case ModelType.AdaTextEmbedding:
                    return AdaTextEmbedding;
                case ModelType.CushmanCode:
                    return CushmanCode;
                case ModelType.DavinciCode:
                    return DavinciCode;
                case ModelType.BabbageText:
                    return BabbageText;
                case ModelType.CurieText:
                    return CurieText;
                case ModelType.AdaText:
                    return AdaText;
                case ModelType.TextModerationStable:
                    return TextModerationStable;
                case ModelType.TextModerationLatest:
                    return TextModerationLatest;
                default:
                    return DefaultModel;
            }
        }
    }
}
