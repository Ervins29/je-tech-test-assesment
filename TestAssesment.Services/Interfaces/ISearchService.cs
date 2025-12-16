using TestAssesment.Data.Services.Models;
using TestAssesment.Integrations.Omdb.Models;

namespace TestAssesment.Services.Services;

public interface ISearchService
{
    Task<OmdbMovie> SearchMovie(string title);

    Task<List<SavedSearchDto>> GetRecentSearches();
}