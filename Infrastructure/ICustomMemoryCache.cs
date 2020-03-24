using System;

namespace FractalSolutions.Api.Infrastructure
{
    public interface ICustomMemoryCache
    {
        TItem Get<TItem>(string key);
        void Set<TItem>(string key, TItem cacheItem);
    }
}