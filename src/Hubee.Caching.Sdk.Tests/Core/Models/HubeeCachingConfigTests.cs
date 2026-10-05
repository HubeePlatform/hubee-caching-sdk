using Hubee.Caching.Sdk.Core.Models;
using Hubee.Caching.Sdk.Tests.Core.TestData;
using System;
using Xunit;

namespace Hubee.Caching.Sdk.Tests.Core.Models
{
    public class HubeeCachingConfigTests
    {
        [Theory]
        [InlineData("invalid_port")]
        [InlineData("invalid_host")]
        [InlineData("invalid_password")]
        [InlineData("invalid_provider")]
        [InlineData("invalid_default_expires_in")]
        [InlineData("invalid_operation_timeout")]
        public void Should_DoNotAcceptSettings_When_Invalid(string nameSetting)
        {
            var config = HubeeCachingConfigData.GetConfig(nameSetting, "Invalid");
            Assert.Throws<InvalidOperationException>(() => config.CheckConfig());
        }

        [Fact]
        public void Should_KeepResilienceOptionsDisabled_When_NotConfigured()
        {
            var config = HubeeCachingConfigData.GetConfig("valid_config", "Valid");

            Assert.Null(config.OperationTimeoutMilliseconds);
            Assert.False(config.FailFastWhenDisconnected);
        }

        [Fact]
        public void Should_BindResilienceOptions_When_Configured()
        {
            var config = HubeeCachingConfigData.GetConfig("valid_config_with_resilience", "Valid");

            config.CheckConfig();

            Assert.Equal(100, config.OperationTimeoutMilliseconds);
            Assert.True(config.FailFastWhenDisconnected);
        }

        [Theory]
        [InlineData("valid_config")]
        public void Should_AcceptSettings_When_valid(string nameSetting)
        {
            var config = HubeeCachingConfigData.GetConfig(nameSetting, "Valid");

            config.CheckConfig();

            Assert.NotEqual(string.Empty, config.CacheProvider);
            Assert.NotEqual(CacheProviderType.Undefined, config.CacheProviderType);   
            Assert.NotEqual(string.Empty, config.Host);
            Assert.NotEqual(string.Empty, config.Password);
            Assert.True(int.TryParse(config.Port, out _));
            Assert.True(TimeSpan.TryParse(config.DefaultExpiresIn, out _));
        }
    }
}