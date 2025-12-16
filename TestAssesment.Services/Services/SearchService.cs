using Microsoft.Extensions.Logging;
using TestAssesment.Data.Services;
using TestAssesment.Data.Services.Models;
using TestAssesment.Integrations.Omdb.Interfaces;
using TestAssesment.Integrations.Omdb.Models;

namespace TestAssesment.Services.Services;

public class SearchService(IOmdbClient client, IMovieSearchStorageService movieSearchStorageService, ILogger<SearchService> logger) : ISearchService
{
    public async Task<OmdbMovie> SearchMovie(string title)
    {
        try
        {
            var searchResult = await client.SearchMovies(title: title);

            logger.LogInformation($"Search for {title} returned {searchResult.Title}");

            if (!searchResult.Response) return searchResult;
            await movieSearchStorageService.SaveMovieSearch(searchResult.Title, searchResult.ImdbId);
            
            logger.LogInformation($"Movie {searchResult.Title} saved to recent searches");

            return searchResult;
        }
        catch (Exception e)
        {
            var errorMessage = $"Error while searching or saving movie {title}";
            
            logger.LogError(e, $"Error while searching or saving movie {title}");
            return new OmdbMovie
            {
                Response = false,
                Error = errorMessage
            };
        }
    }

    public async Task<List<SavedSearchDto>> GetRecentSearches()
    {
        var savedSearches = await movieSearchStorageService.GetRecentSearches();

        return savedSearches.ToList();
    }
}

public interface ISearchService
{
    Task<OmdbMovie> SearchMovie(string title);

    Task<List<SavedSearchDto>> GetRecentSearches();
}