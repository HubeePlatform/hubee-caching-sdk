using Hubee.Caching.Sdk.Core.Models;
using StackExchange.Redis;
using System;

namespace Hubee.Caching.Sdk.Core.Helpers
{
    public class RedisHelper
    {
        private static RedisHelper _instance;
        private static readonly object _lock = new object();
        private readonly ConnectionMultiplexer _connection;
        private readonly HubeeCachingConfig _config;

        private RedisHelper(HubeeCachingConfig config)
        {
            _config = config;

            var connectionString = _config.GetConnectionString();
            _connection = ConnectionMultiplexer.Connect(connectionString);
        }

        public static RedisHelper Initialize(HubeeCachingConfig config)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new RedisHelper(config);
                }
            }

            return _instance;
        }

        public static RedisHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("RedisHelper not initialized. Call Initialize() first.");
                }
                return _instance;
            }
        }

        public IDatabase Database => _connection.GetDatabase();

        public void Dispose()
        {
            if (_connection != null && _connection.IsConnected)
            {
                _connection.Dispose();
            }
        }
    }
}
