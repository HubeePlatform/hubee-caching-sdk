using Hubee.Caching.Sdk.Core.Helpers;
using System;

namespace Hubee.Caching.Sdk.Core.Models
{
    public class HubeeCachingConfig
    {
        public string CacheProvider { get; set; }
        public CacheProviderType CacheProviderType => EnumHelper.Parse<CacheProviderType>(this.CacheProvider);
        public string DefaultExpiresIn { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Password { get; set; }

        public TimeSpan GetDefaultExpiresIn()
        {
            return TimeSpan.Parse(this.DefaultExpiresIn);
        }

        public HubeeCachingConfig GetValueInEnvironmentVariable()
        {
            if (!string.IsNullOrEmpty(this.Host) && !string.IsNullOrEmpty(this.Port) && !string.IsNullOrEmpty(this.Password))
                return this;

            this.Host = Environment.GetEnvironmentVariable("HUBEE_CACHING_HOST");
            this.Port = Environment.GetEnvironmentVariable("HUBEE_CACHING_PORT");
            this.Password = Environment.GetEnvironmentVariable("HUBEE_CACHING_PASSWORD");
            return this;
        }

        public void CheckConfig()
        {
            var isInvalid = string.IsNullOrEmpty(this.CacheProvider) ||
                            this.CacheProviderType.Equals(CacheProviderType.Undefined) ||
                            string.IsNullOrEmpty(this.Host) ||
                            !int.TryParse(this.Port, out _) ||
                            !TimeSpan.TryParse(this.DefaultExpiresIn, out _) ||
                            string.IsNullOrEmpty(this.Password);

            if (isInvalid)
                throw new InvalidOperationException($"Please, configure appsettings with a {nameof(HubeeCachingConfig)} section");
        }
    }
}