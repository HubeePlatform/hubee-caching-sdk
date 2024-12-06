using Hubee.Caching.Sdk.Core.Interfaces;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Hubee.Caching.Sdk.Core.Models;
using StackExchange.Redis;
using System.Text.Json;
using Hubee.Caching.Sdk.Core.Helpers;

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
                string jsonData = await GetRedisDatabase().StringGetAsync(key);

                if (jsonData is null)
                    return default;

                return JsonSerializer.Deserialize<T>(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.StackTrace);
                return default;
            }
        }

        public async Task Set<T>(string key, T value, TimeSpan? expiresIn = null)
        {
            try
            {
                if (value is null) return;

                var expiresInCache = expiresIn ?? _cachingConfig.Value.GetDefaultExpiresIn();

                var redisDatabase = GetRedisDatabase();
                string jsonData = JsonSerializer.Serialize(value);

                await redisDatabase.StringSetAsync(key, jsonData, expiresInCache);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.StackTrace);
            }
        }

        private IDatabase GetRedisDatabase()
        {
            _cachingConfig.Value.GetValueInEnvironmentVariable();

            return RedisHelper.Initialize(_cachingConfig.Value).Database;
        }
    }
}