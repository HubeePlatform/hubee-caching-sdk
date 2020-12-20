
using Hubee.Caching.Sdk.Core.Exceptions;
using Hubee.Caching.Sdk.Core.Interfaces;
using Hubee.Caching.Sdk.Core.Models;
using Hubee.Caching.Sdk.Infra.Redis;

namespace Hubee.Caching.Sdk.Core.Factories
{
    internal class CachingProviderFactory
    {
        public ICachingProvider GetByConfig(HubeeCachingConfig config)
        {
            return config.CacheProviderType switch
            {
                CacheProviderType.Redis => new RedisProvider(),
                _ => throw new CacheProviderNotSupportedException(config.CacheProvider)
            };
        }
    }
}