using System;

namespace Azure.Ai.OpenAi
{
    public sealed class OpenAiConfiguration
    {
        public string Uri { get; }
        public string CompletionUri { get; }
        public string ChatUri { get; }
        public string EditUri { get; }
        public string EmbeddingUri { get; }
        public string FileUri { get; }
        public string ModelUri { get; }
        public string ModerationUri { get; }
        public string ImageUri { get; }
        internal OpenAiConfiguration(OpenAiSettings settings)
        {
            if (settings.Azure.ResourceName != null)
            {
                if (settings.Azure.DeploymentId == null)
                    throw new ArgumentNullException($"When you set an Azure resource name you have to add a {nameof(OpenAiSettings.Azure.DeploymentId)} in configuration setup.");

                settings.Version ??= "2022-12-01";
                Uri = $"https://{settings.Azure.ResourceName}.OpenAi.Microsoft.com/openai/deployments/{settings.Azure.DeploymentId}/{{0}}?api-version={settings.Version}";
            }
            else
            {
                settings.Version ??= "v1";
                Uri = $"https://api.openai.com/{settings.Version}/{{0}}";
            }
            CompletionUri = string.Format(Uri, "completions");
            ChatUri = string.Format(Uri, "chat/completions");
            EditUri = string.Format(Uri, "edits");
            EmbeddingUri = string.Format(Uri, "embeddings");
            FileUri = string.Format(Uri, "files");
            ModelUri = string.Format(Uri, "models");
            ModerationUri = string.Format(Uri, "moderations");
            ImageUri = string.Format(Uri, "images");
        }
    }
}
