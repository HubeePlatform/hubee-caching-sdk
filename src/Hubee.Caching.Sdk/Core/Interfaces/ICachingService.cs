using System;
using System.Threading.Tasks;

namespace Hubee.Caching.Sdk.Core.Interfaces
{
    public interface ICachingService
    {
        Task Set<T>(string key, T value, TimeSpan? expiresIn = null);
        Task<T> Get<T>(string key);
    }
}