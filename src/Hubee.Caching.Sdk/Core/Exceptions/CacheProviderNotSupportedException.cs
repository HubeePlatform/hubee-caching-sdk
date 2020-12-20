using System;

namespace Hubee.Caching.Sdk.Core.Exceptions
{
    public class CacheProviderNotSupportedException : Exception
    {
        public CacheProviderNotSupportedException(string typeName) : base($"Cache provider {typeName} not supported")
        {

        }
    }
}