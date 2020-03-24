using FractalSolutions.Api.HttpClients;
using FractalSolutions.Api.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FractalSolutions.Api.Configuration
{
    public static class ServiceClientConfigurator
    {
        public static IServiceCollection ConfigureHttpServices(this IServiceCollection services, IServiceConfigCollection serviceConfigCollection, IClientSettings clientSettings)
        {
            services.AddScoped<AddTrueLayerTokenHandler>();

            services.AddHttpClient<ITrueLayerAuthClient, TrueLayerAuthClient>(c =>
            {
                c.BaseAddress = new Uri(serviceConfigCollection[ServiceConfigNames.TrueLayerAuth].BaseUrl);
                c.Timeout = TimeSpan.FromSeconds(serviceConfigCollection[ServiceConfigNames.TrueLayerAuth].TimeOutSeconds);
            });

            services.AddHttpClient<ITrueLayerDataClient, TrueLayerDataClient>(c =>
            {
                c.BaseAddress = new Uri(serviceConfigCollection[ServiceConfigNames.TrueLayerData].BaseUrl);
                c.Timeout = TimeSpan.FromSeconds(serviceConfigCollection[ServiceConfigNames.TrueLayerData].TimeOutSeconds);
            }).AddHttpMessageHandler<AddTrueLayerTokenHandler>();

            return services;    
        }      
    }
}
