using Hubee.Caching.Sdk.Core.Interfaces;
using Hubee.Caching.Sdk.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hubee.Caching.Sdk.Infra.Redis
{
    internal class RedisProvider : ICachingProvider
    {
        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HubeeCachingConfig>(configuration.GetSection("HubeeCachingConfig"));
            services.AddSingleton<ICachingService, RedisCachingService>();
        }
    }
}