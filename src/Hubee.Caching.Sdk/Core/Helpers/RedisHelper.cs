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
            _connection = ConnectionMultiplexer.Connect(CreateOptions(_config));
        }

        internal static ConfigurationOptions CreateOptions(HubeeCachingConfig config)
        {
            var options = ConfigurationOptions.Parse(config.GetConnectionString());
            options.AbortOnConnectFail = false;

            if (config.OperationTimeoutMilliseconds is int timeout)
            {
                options.AsyncTimeout = timeout;
                options.SyncTimeout = timeout;
            }

            if (config.FailFastWhenDisconnected)
            {
                options.BacklogPolicy = BacklogPolicy.FailFast;
            }

            return options;
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
