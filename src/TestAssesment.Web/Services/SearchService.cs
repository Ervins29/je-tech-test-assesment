using TestAssesment.Data.Services;
using TestAssesment.Data.Services.Models;
using TestAssesment.Integrations.Omdb.Interfaces;
using TestAssesment.Integrations.Omdb.Models;

namespace TestAssesment.Web.Services;

public class SearchService(IOmdbClient client, IMovieSearchService movieSearchService)
{
    public async Task<OmdbMovie> SearchMovie(string title)
    {
        var searchResult = await client.SearchMovies(title: title);

        if (searchResult.Response)
        {
            await movieSearchService.SaveMovieSearch(searchResult.Title, searchResult.ImdbId);
        }

        return searchResult;
    }

    public async Task<List<SavedSearchDto>> GetRecentSearches()
    {
        var savedSearches = await movieSearchService.GetRecentSearches();

        return savedSearches.ToList();
    }
}