using Hubee.Caching.Sdk.Core.Helpers;
using Hubee.Caching.Sdk.Tests.Core.TestData;
using System.Net;
using Xunit;

namespace Hubee.Caching.Sdk.Tests.Core.Helpers
{
    public class RedisHelperTests
    {
        [Fact]
        public void Should_NotAbortOnConnectFail_When_CreatingOptions()
        {
            var config = HubeeCachingConfigData.GetConfig("valid_config", "Valid");

            var options = RedisHelper.CreateOptions(config);

            Assert.False(options.AbortOnConnectFail);
        }

        [Fact]
        public void Should_KeepLibraryDefaults_When_ResilienceOptionsAreNotConfigured()
        {
            var config = HubeeCachingConfigData.GetConfig("valid_config", "Valid");

            var options = RedisHelper.CreateOptions(config);

            Assert.Equal(5000, options.AsyncTimeout);
            Assert.Equal(5000, options.SyncTimeout);
            Assert.True(options.BacklogPolicy.QueueWhileDisconnected);
        }

        [Fact]
        public void Should_ApplyResilienceOptions_When_Configured()
        {
            var config = HubeeCachingConfigData.GetConfig("valid_config_with_resilience", "Valid");

            var options = RedisHelper.CreateOptions(config);

            Assert.Equal(100, options.AsyncTimeout);
            Assert.Equal(100, options.SyncTimeout);
            Assert.False(options.BacklogPolicy.QueueWhileDisconnected);
        }

        [Fact]
        public void Should_KeepEndpointAndPassword_When_CreatingOptions()
        {
            var config = HubeeCachingConfigData.GetConfig("valid_config", "Valid");

            var options = RedisHelper.CreateOptions(config);

            Assert.Equal(new DnsEndPoint("localhost", 3000), options.EndPoints[0]);
            Assert.Equal("password", options.Password);
        }
    }
}
