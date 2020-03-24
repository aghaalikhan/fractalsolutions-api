namespace FractalSolutions.Api.Configuration
{
    public class ClientSettings: IClientSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
    }
}