namespace FractalSolutions.Api.Configuration
{
    public interface IClientSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
    }
}
