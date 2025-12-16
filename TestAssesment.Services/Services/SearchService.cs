using Microsoft.Extensions.Logging;
using TestAssesment.Data.Services;
using TestAssesment.Data.Services.Models;
using TestAssesment.Integrations.Omdb.Interfaces;
using TestAssesment.Integrations.Omdb.Models;

namespace TestAssesment.Services.Services;

public class SearchService(
    IOmdbClient client,
    IMovieSearchStorageService movieSearchStorageService,
    ILogger<SearchService> logger) : ISearchService
{
    public async Task<OmdbMovie> SearchMovie(string title)
    {
        try
        {
            var searchResult = await client.SearchMovies(title);

            logger.LogInformation("Search for {Title} returned {ResultTitle}", title, searchResult.Title);

            if (!searchResult.Response)
            {
                logger.LogWarning("Search for {Title} failed with error: {Error}", title, searchResult.Error);
                return searchResult;
            }

            if (string.IsNullOrWhiteSpace(searchResult.Title) || string.IsNullOrWhiteSpace(searchResult.ImdbId))
            {
                logger.LogWarning("Search for {Title} returned incomplete data", title);
                return new OmdbMovie
                {
                    Response = false,
                    Error = "Incomplete movie data returned from API"
                };
            }

            await movieSearchStorageService.SaveMovieSearch(searchResult.Title, searchResult.ImdbId);

            logger.LogInformation("Movie {MovieTitle} saved to recent searches", searchResult.Title);

            return searchResult;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP error while searching for movie {Title}", title);
            return new OmdbMovie
            {
                Response = false,
                Error = "Unable to connect to movie database service"
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while searching for movie {Title}", title);
            return new OmdbMovie
            {
                Response = false,
                Error = "An unexpected error occurred while searching"
            };
        }
    }

    public async Task<List<SavedSearchDto>> GetRecentSearches()
    {
        return await movieSearchStorageService.GetRecentSearches();
    }
}