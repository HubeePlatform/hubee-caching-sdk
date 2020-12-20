using Hubee.Caching.Sdk.Core.Factories;
using Hubee.Caching.Sdk.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hubee.Caching.Sdk.Core.Extensions
{
    public static class Extensions
    {
        private static readonly CachingProviderFactory _cachingProviderFactory = new CachingProviderFactory();

        public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            var config = new HubeeCachingConfig();
            configuration.GetSection("HubeeCachingConfig").Bind(config);

            config.GetValueInEnvironmentVariable().CheckConfig();

            var cachingProvider = _cachingProviderFactory.GetByConfig(config);
            cachingProvider.Register(services, configuration);

            return services;
        }
    }
}