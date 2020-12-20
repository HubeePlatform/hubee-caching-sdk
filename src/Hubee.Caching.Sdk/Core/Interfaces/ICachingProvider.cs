using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hubee.Caching.Sdk.Core.Interfaces
{
    internal interface ICachingProvider
    {
        void Register(IServiceCollection services, IConfiguration configuration);
    }
}