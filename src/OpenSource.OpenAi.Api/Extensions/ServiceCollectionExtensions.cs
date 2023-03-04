using System;
using System.Net.Http;
using System.Net.Http.Headers;
using OpenSource.OpenAi;
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
using Polly;
using Polly.Extensions.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3928:Parameter names used into ArgumentException constructors should match an existing one ", Justification = "The parameter is in the root of the setting class.")]
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
