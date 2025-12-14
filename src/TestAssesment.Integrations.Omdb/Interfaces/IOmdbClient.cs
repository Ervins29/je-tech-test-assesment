using Refit;
using TestAssesment.Integrations.Omdb.Models;

namespace TestAssesment.Integrations.Omdb.Interfaces;

public interface IOmdbClient
{
    [Get("/")]
    public Task<OmdbMovie> SearchMovies([AliasAs("t")]string? title = null, [AliasAs("i")]string? id = null);
}