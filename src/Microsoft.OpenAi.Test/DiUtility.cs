using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenAi.Api;

namespace Microsoft.OpenAi.Test
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
            var apiKey = configuration["Azure:ApiKey"];
            var resourceName = configuration["Azure:ResourceName"];
            var deploymentId = configuration["Azure:DeploymentId"];
            services.AddOpenAi(settings =>
            {
                settings.ApiKey = apiKey;
                settings.Azure.ResourceName = resourceName;
                settings.Azure.DeploymentId = deploymentId;
            });
            return services;
        }
        public static IServiceProvider Finalize(this IServiceCollection services, out IServiceProvider serviceProvider)
            => serviceProvider = services.BuildServiceProvider().CreateScope().ServiceProvider;
        public static IOpenAiApi GetOpenAi()
        {
            var services = CreateDependencyInjectionWithConfiguration(out var configuration);
            _ = services.Finalize(out var serviceProvider);
            return serviceProvider.CreateScope().ServiceProvider.GetService<IOpenAiApi>();
        }
    }
}
