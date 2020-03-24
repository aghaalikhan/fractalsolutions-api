using Microsoft.Extensions.Configuration;

namespace FractalSolutions.Api.Extensions
{
    public static class ConfigurationExtension
    {
        public static T BindConfigurationSection<T>(this IConfiguration configuration, string section) where T : new()
        {
            var settings = new T();
            configuration.GetSection(section).Bind(settings);
            return settings;
        }
    }
}
