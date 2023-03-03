using Azure.Ai.OpenAi;
using System;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenAi(this IServiceCollection services, Action<OpenAiSettings> settings)
        {
            var openAiSettings = new OpenAiSettings();
            settings.Invoke(openAiSettings);
            if (openAiSettings.ApiKey == null)
                throw new ArgumentNullException($"{nameof(OpenAiSettings.ApiKey)} is empty.");

            services.AddSingleton(new OpenAiConfiguration(openAiSettings));
            var httpClientBuilder = services.AddHttpClient(OpenAiSettings.HttpClientName, client =>
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAiSettings.ApiKey);
                if (!string.IsNullOrEmpty(openAiSettings.OrganizationName))
                    client.DefaultRequestHeaders.Add("OpenAI-Organization", openAiSettings.OrganizationName);

            });
            if (openAiSettings.RetryPolicy)
            {
                var defaultPolicy = openAiSettings.CustomRetryPolicy ?? Policy<HttpResponseMessage>
                   .Handle<HttpRequestException>()
                   .OrTransientHttpError()
                   .AdvancedCircuitBreakerAsync(0.5, TimeSpan.FromSeconds(10), 10, TimeSpan.FromSeconds(15));
                httpClientBuilder
                     .AddPolicyHandler(defaultPolicy);
            }
            services
                .AddScoped<IOpenAiApi, OpenAiApi>()
                .AddScoped<IOpenAiEmbeddingApi, OpenAiEmbeddingApi>()
                .AddScoped<IOpenAiFileApi, OpenAiFileApi>()
                .AddScoped<IOpenAiModelApi, OpenAiModelApi>()
                .AddScoped<IOpenAiModerationApi, OpenAiModerationApi>()
                .AddScoped<IOpenAiImageApi, OpenAiImageApi>()
                .AddScoped<IOpenAiCompletionApi, OpenAiCompletionApi>();
            return services;
        }
    }
}
