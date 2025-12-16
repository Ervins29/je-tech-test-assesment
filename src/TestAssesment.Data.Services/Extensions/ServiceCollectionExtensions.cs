using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestAssesment.Data.Services.Configurations;

namespace TestAssesment.Data.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<SearchConfiguration>()
            .Bind(configuration.GetSection(SearchConfiguration.SectionName));

        services.AddScoped<IMovieSearchStorageService, MovieSearchStorageService>();

        return services;
    }
}