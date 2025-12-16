using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;
using System.Text.Json;
using System.Text.Json.Serialization;
using TestAssesment.Integrations.Omdb.Configurations;
using TestAssesment.Integrations.Omdb.Converters;
using TestAssesment.Integrations.Omdb.Handlers;
using TestAssesment.Integrations.Omdb.Interfaces;

namespace TestAssesment.Integrations.Omdb.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOmdbApi(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddOptions<OmdbConfiguration>()
            .Bind(configuration.GetSection(OmdbConfiguration.SectionName));

        services.AddTransient<HttpApiKeyHandler>();

        services
            .AddRefitClient<IOmdbClient>()
            .ConfigureHttpClient((sp, c) =>
            {
                var omdbConfiguration = sp.GetRequiredService<IOptions<OmdbConfiguration>>().Value;

                if (string.IsNullOrEmpty(omdbConfiguration.BaseUrl) || string.IsNullOrEmpty(omdbConfiguration.ApiKey))
                    throw new Exception("OmdbApi configuration is invalid");

                c.BaseAddress = new Uri(omdbConfiguration.BaseUrl);
            })
            .AddHttpMessageHandler<HttpApiKeyHandler>();

        return services;
    }
}