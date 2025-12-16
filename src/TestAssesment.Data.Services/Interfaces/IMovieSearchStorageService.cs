using TestAssesment.Data.Services.Models;

namespace TestAssesment.Data.Services;

public interface IMovieSearchStorageService
{
    Task SaveMovieSearch(string movieTitle, string imdbMovieId);

    Task<List<SavedSearchDto>> GetRecentSearches();
}