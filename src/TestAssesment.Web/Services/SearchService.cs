using TestAssesment.Data.Services;
using TestAssesment.Data.Services.Models;
using TestAssesment.Integrations.Omdb.Interfaces;
using TestAssesment.Integrations.Omdb.Models;

namespace TestAssesment.Web.Services;

public class SearchService(IOmdbClient client, IMovieSearchStorageService movieSearchStorageService)
{
    public async Task<OmdbMovie> SearchMovie(string title)
    {
        var searchResult = await client.SearchMovies(title: title);

        if (searchResult.Response)
        {
            await movieSearchStorageService.SaveMovieSearch(searchResult.Title, searchResult.ImdbId);
        }

        return searchResult;
    }

    public async Task<List<SavedSearchDto>> GetRecentSearches()
    {
        var savedSearches = await movieSearchStorageService.GetRecentSearches();

        return savedSearches.ToList();
    }
}