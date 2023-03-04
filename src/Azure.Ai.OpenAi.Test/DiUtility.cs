using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Ai.OpenAi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Azure.OpenAi.Test
{
    internal static class DiUtility
    {
        private sealed class ForUserSecrets { }
        public static IServiceCollection CreateDependencyInjectionWithConfiguration(out IConfiguration configuration)
        {
            var services = new ServiceCollection();
            configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.test.json")
               .AddUserSecrets<ForUserSecrets>()
               .AddEnvironmentVariables()
               .Build();
            services.AddSingleton(configuration);
            var apiKey = configuration["OpenAi:ApiKey"];
            services.AddOpenAi(settings =>
            {
                settings.ApiKey = apiKey;
            });
            return services;
        }
        public static IServiceCollection CreateDependencyInjectionWithConfigurationForAzure(out IConfiguration configuration)
        {
            var services = new ServiceCollection();
            configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.test.json")
               .AddUserSecrets<ForUserSecrets>()
               .AddEnvironmentVariables()
               .Build();
            services.AddSingleton(configuration);
            var apiKey = configuration["Azure:ApiKey"];
            var resourceName = configuration["Azure:ResourceName"];
            var deploymentId = configuration["Azure:DeploymentId"];
            services.AddOpenAi(settings =>
            {
                settings.ApiKey = apiKey;
                settings.Azure.ResourceName = resourceName;
                settings
                    .Azure
                    .AddDeploymentModel(deploymentId, Ai.OpenAi.Models.TextModelType.CurieText);
            });
            return services;
        }
        public static IServiceProvider Finalize(this IServiceCollection services, out IServiceProvider serviceProvider)
            => serviceProvider = services.BuildServiceProvider().CreateScope().ServiceProvider;
        public static IOpenAiApi GetOpenAi()
        {
            var services = CreateDependencyInjectionWithConfiguration(out _);
            _ = services.Finalize(out var serviceProvider);
            return serviceProvider.CreateScope().ServiceProvider.GetService<IOpenAiApi>();
        }
        public static IOpenAiApi GetOpenAiForAzure()
        {
            var services = CreateDependencyInjectionWithConfigurationForAzure(out _);
            _ = services.Finalize(out var serviceProvider);
            return serviceProvider.CreateScope().ServiceProvider.GetService<IOpenAiApi>();
        }
    }
}
