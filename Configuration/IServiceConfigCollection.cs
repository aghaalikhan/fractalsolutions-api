namespace FractalSolutions.Api.Configuration
{
    public interface IServiceConfigCollection
    {
        ServiceConfig this[string name] { get; }
    }
}
