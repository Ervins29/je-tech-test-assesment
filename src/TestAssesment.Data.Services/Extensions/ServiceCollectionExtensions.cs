using Microsoft.Extensions.DependencyInjection;

namespace TestAssesment.Data.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddScoped<IMovieSearchService, MovieSearchService>();

        return services;
    }
}