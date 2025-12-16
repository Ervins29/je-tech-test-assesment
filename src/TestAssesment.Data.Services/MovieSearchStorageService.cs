using Microsoft.EntityFrameworkCore;
using TestAssesment.Data.DataAccess;
using TestAssesment.Data.DataAccess.Models;
using TestAssesment.Data.Services.Models;

namespace TestAssesment.Data.Services;

public class MovieSearchStorageService(DataContext dataContext) : IMovieSearchStorageService
{
    private DbSet<MovieSearchQuery> MovieSearches => dataContext.MovieSearchQueries;

    public async Task<IEnumerable<SavedSearchDto>> GetRecentSearches()
    {
        //Not sure if this needs to be sorted? I assume it not
        var results = await MovieSearches.AsNoTracking().ToListAsync();

        return results.Select(x => new SavedSearchDto(x.MovieTitle, x.ImdbMovieId));
    }

    public async Task SaveMovieSearch(string searchQuery, string imdbMovieId)
    {
        var savedMovieSearches = await MovieSearches
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

public interface IMovieSearchStorageService
{
    Task SaveMovieSearch(string searchQuery, string imdbMovieId);

    Task<IEnumerable<SavedSearchDto>> GetRecentSearches();
}