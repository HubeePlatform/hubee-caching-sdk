using Hubee.Caching.Sdk.Core.Interfaces;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Hubee.Caching.Sdk.Core.Models;
using StackExchange.Redis;
using System.Text.Json;



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
                var redisClient = GetRedisClient();
                var value = Task.FromResult(await redisClient.StringGetAsync(key)).ToString();

                if (value == string.Empty)
                    return default;

                return JsonSerializer.Deserialize<T>(value);
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

                var redisClient = GetRedisClient();

                var serializedValue = JsonSerializer.Serialize(value);

                await Task.FromResult(redisClient.StringSetAsync(key, serializedValue, expiresInCache));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.StackTrace);
                throw;
            }
        }

        private IDatabase GetRedisClient()
        {
            _cachingConfig.Value.GetValueInEnvironmentVariable();

            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { $"{_cachingConfig.Value.Host}:{_cachingConfig.Value.Port}" },
                Password = _cachingConfig.Value.Password,
                AbortOnConnectFail = false, 
                ConnectRetry = 5, 
            };

            return new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions)).Value.GetDatabase();
        }
    }
}