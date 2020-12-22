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
        public void Should_DoNotAcceptSettings_When_Invalid(string nameSetting)
        {
            var config = HubeeCachingConfigData.GetConfig(nameSetting, "Invalid");
            Assert.Throws<InvalidOperationException>(() => config.CheckConfig());
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