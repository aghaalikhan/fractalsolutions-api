using FractalSolutions.Api.Services;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace FractalSolutions.Api.Infrastructure
{
    public class CustomMemoryCache : ICustomMemoryCache
    {
        private readonly IDateTimeService _dateTimeService;
        private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public CustomMemoryCache(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }

        public TItem Get<TItem>(string key)
        {
            return _cache.Get<TItem>(key);
        }

        public void Set<TItem>(string key, TItem cacheItem)
        {            
            _cache.Set(key, cacheItem, _dateTimeService.UtcNow.AddMinutes(5));
        }       
    }
}
