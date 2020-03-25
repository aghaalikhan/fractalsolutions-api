using FractalSolutions.Api.Configuration;
using FractalSolutions.Api.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace FractalSolutions.Api.Infrastructure
{
    public class CustomMemoryCache : ICustomMemoryCache
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly ICacheSettings _cacheSettings;
        private IMemoryCache _cache;

        public CustomMemoryCache(IDateTimeService dateTimeService, ICacheSettings cacheSettings)
        {
            _dateTimeService = dateTimeService;
            _cacheSettings = cacheSettings;
            _cache = new MemoryCache(new MemoryCacheOptions
            {
                ExpirationScanFrequency = TimeSpan.FromMinutes(_cacheSettings.ExpirationScanFrequencyMinutes)
            });
        }

        public TItem Get<TItem>(string key)
        {
            return _cache.Get<TItem>(key);
        }
     
        public void Set<TItem>(string key, TItem cacheItem)
        {
            var expirationTime = _dateTimeService.UtcNow.AddMinutes(_cacheSettings.ExpirationScanFrequencyMinutes);
            _cache.Set(key, cacheItem, expirationTime);
        }       
    }
}
