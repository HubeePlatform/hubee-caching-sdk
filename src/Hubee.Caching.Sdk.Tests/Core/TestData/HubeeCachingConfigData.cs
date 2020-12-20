using Hubee.Caching.Sdk.Core.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Hubee.Caching.Sdk.Tests.Core.TestData
{
    public static class HubeeCachingConfigData
    {
        private static IConfiguration GetConfiguration(string nameSetting, string path = "")
        {
            return new ConfigurationBuilder()
                  .SetBasePath($"{Directory.GetCurrentDirectory()}\\Core\\TestData\\Settings\\{path}")
                  .AddJsonFile($"{nameSetting}.json")
                  .Build();
        }

        public static HubeeCachingConfig GetConfig(string nameSetting, string path = "")
        {
            var configuration = GetConfiguration(nameSetting, path);
            var cachingConfig = new HubeeCachingConfig();

            configuration.GetSection("HubeeCachingConfig").Bind(cachingConfig);
            return cachingConfig;
        }
    }
}