using Microsoft.EntityFrameworkCore;
using TestAssesment.Data.DataAccess;
using TestAssesment.Data.DataAccess.Models;
using TestAssesment.Data.Services.Models;

namespace TestAssesment.Data.Services;

public class MovieSearchService(DataContext dataContext) : IMovieSearchService
{
    private DbSet<MovieSearchQuery> MovieSearches => dataContext.MovieSearchQueries;

    public async Task<IEnumerable<SavedSearchDto>> GetRecentSearches()
    {
        var results = await MovieSearches.ToListAsync();

        return results.Select(x => new SavedSearchDto(x.MovieTitle, x.ImdbMovieId));
    }

    public async Task SaveMovieSearch(string searchQuery, string imdbMovieId)
    {
        var savedMovieSearches = await MovieSearches
            .AsNoTracking()
            .OrderByDescending(x => x.TimeStamp)
            .ToListAsync();

        var existingSearchQuery = savedMovieSearches.FirstOrDefault(x => x.MovieTitle == searchQuery);

        if (existingSearchQuery is not null)
        {
            return;
        }

        if (savedMovieSearches.Count == 5)
        {
            var oldestSearch = savedMovieSearches.LastOrDefault();

            if (oldestSearch is not null)
            {
                MovieSearches.Remove(oldestSearch);
            }
        }
        
        await MovieSearches.AddAsync(new MovieSearchQuery { MovieTitle = searchQuery, ImdbMovieId = imdbMovieId});
        
        await dataContext.SaveChangesAsync();
    }
}

public interface IMovieSearchService
{
    Task SaveMovieSearch(string searchQuery, string imdbMovieId);

    Task<IEnumerable<SavedSearchDto>> GetRecentSearches();
}