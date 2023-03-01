using Microsoft.Extensions.Options;
using Polly;

namespace Microsoft.OpenAi.Api
{
    public sealed class OpenAiSettings
    {
        public string? ApiKey { get; set; }
        public string? OrganizationName { get; set; }
        public string? Version { get; set; }
        public string? AzureResourceName { get; set; }
        public string? DeploymentId { get; set; }
        public bool RetryPolicy { get; set; } = true;
        public IAsyncPolicy<HttpResponseMessage>? CustomRetryPolicy { get; set; }
        internal const string HttpClientName = "openaiclient_rystem";
    }
}
