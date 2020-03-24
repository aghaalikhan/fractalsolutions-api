namespace FractalSolutions.Api.Configuration
{
    public class ServiceConfig
    {
        private const int DefaultTimeoutSeconds = 20;

        private int? _timeOutSeconds;

        public string BaseUrl { get; set; }

        public int TimeOutSeconds
        {
            get => _timeOutSeconds ?? DefaultTimeoutSeconds;
            set => _timeOutSeconds = value;
        }
    }
}
