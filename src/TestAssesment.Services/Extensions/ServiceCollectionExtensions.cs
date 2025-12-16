using Microsoft.Extensions.DependencyInjection;
using TestAssesment.Services.Services;

namespace TestAssesment.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ISearchService, SearchService>();

        return services;
    }
}