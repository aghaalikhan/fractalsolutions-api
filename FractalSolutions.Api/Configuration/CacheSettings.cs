namespace FractalSolutions.Api.Configuration
{
    public class CacheSettings: ICacheSettings
    {
        public int ExpirationScanFrequencyMinutes { get; set; }
        public int DefaultCacheItemLifeSpanMinutes { get; set; }
    }
}
