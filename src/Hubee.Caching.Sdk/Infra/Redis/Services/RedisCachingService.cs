using Hubee.Caching.Sdk.Core.Interfaces;
using System.Threading.Tasks;
using ServiceStack.Redis;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Hubee.Caching.Sdk.Core.Models;

namespace Hubee.Caching.Sdk.Infra.Redis
{
    internal class RedisCachingService : ICachingService
    {
        private readonly ILogger<RedisCachingService> _logger;
        private readonly IOptions<HubeeCachingConfig> _cachingConfig;

        public RedisCachingService(
            ILogger<RedisCachingService> logger,
             IOptions<HubeeCachingConfig> cachingConfig)
        {
            _logger = logger;
            _cachingConfig = cachingConfig;
        }

        public async Task<T> Get<T>(string key)
        {
            try
            {
                using var redisClient = GetRedisClient();
                return await Task.FromResult(redisClient.Get<T>(key));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task Set<T>(string key, T value, TimeSpan? expiresIn = null)
        {
            try
            {
                var expiresInCache = expiresIn ?? _cachingConfig.Value.GetDefaultExpiresIn();

                using var redisClient = GetRedisClient();
                await Task.FromResult(redisClient.Set<T>(key, value, expiresInCache));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.StackTrace);
                throw;
            }
        }

        private RedisClient GetRedisClient()
        {
            _cachingConfig.Value.GetValueInEnvironmentVariable();
            return new RedisClient(_cachingConfig.Value.Host, int.Parse(_cachingConfig.Value.Port), _cachingConfig.Value.Password);
        }
    }
}