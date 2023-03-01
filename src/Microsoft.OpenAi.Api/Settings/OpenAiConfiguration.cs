namespace Microsoft.OpenAi.Api
{
    public sealed class OpenAiConfiguration
    {
        public string Uri { get; }
        public string CompletionUri { get; }
        public string EmbeddingUri { get; }
        public string FileUri { get; }
        public string ModelUri { get; }
        internal OpenAiConfiguration(OpenAiSettings settings)
        {
            if (settings.AzureResourceName != null)
            {
                if (settings.DeploymentId == null)
                    throw new ArgumentNullException($"When you set an Azure resource name you have to add a {nameof(OpenAiSettings.DeploymentId)} in configuration setup.");

                settings.Version ??= "2022-12-01";
                Uri = $"https://{settings.AzureResourceName}.OpenAi.Microsoft.com/openai/deployments/{settings.DeploymentId}/{{0}}?api-version={settings.Version}";
            }
            else
            {
                settings.Version ??= "v1";
                Uri = $"https://api.openai.com/{settings.Version}/{{0}}";
            }
            CompletionUri = string.Format(Uri, "completions");
            EmbeddingUri = string.Format(Uri, "embeddings");
            FileUri = string.Format(Uri, "files");
            ModelUri = string.Format(Uri, "models");
        }
    }
}
