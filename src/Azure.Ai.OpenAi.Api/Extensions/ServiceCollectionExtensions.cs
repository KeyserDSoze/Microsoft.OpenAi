using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Azure.Ai.OpenAi;
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
using Polly;
using Polly.Extensions.Http;

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
                if (openAiSettings.Azure.HasConfiguration)
                    client.DefaultRequestHeaders.Add("api-key", openAiSettings.ApiKey);
                else
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
                .AddScoped<IOpenAiAudioApi, OpenAiAudioApi>()
                .AddScoped<IOpenAiModelApi, OpenAiModelApi>()
                .AddScoped<IOpenAiModerationApi, OpenAiModerationApi>()
                .AddScoped<IOpenAiImageApi, OpenAiImageApi>()
                .AddScoped<IOpenAiFineTuneApi, OpenAiFineTuneApi>()
                .AddScoped<IOpenAiEditApi, OpenAiEditApi>()
                .AddScoped<IOpenAiChatApi, OpenAiChatApi>()
                .AddScoped<IOpenAiCompletionApi, OpenAiCompletionApi>();
            return services;
        }
    }
}
