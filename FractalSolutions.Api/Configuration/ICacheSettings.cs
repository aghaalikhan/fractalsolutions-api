namespace FractalSolutions.Api.Configuration
{
    public interface ICacheSettings
    {
        public int ExpirationScanFrequencyMinutes { get; set; }
        public int DefaultCacheItemLifeSpanMinutes { get; set; }        
    }
}
