using Microsoft.EntityFrameworkCore;
using TestAssesment.Data.DataAccess;
using TestAssesment.Data.DataAccess.Models;
using TestAssesment.Data.Services.Models;

namespace TestAssesment.Data.Services;

public class MovieSearchStorageService(DataContext dataContext) : IMovieSearchStorageService
{
    private DbSet<MovieSearchQuery> MovieSearches => dataContext.MovieSearchQueries;

    public const int MaxSearchCount = 5;
    
    public async Task<List<SavedSearchDto>> GetRecentSearches()
    {
        var results = await MovieSearches.AsNoTracking().ToListAsync();

        return results.Select(x => new SavedSearchDto(x.MovieTitle, x.ImdbMovieId)).ToList();
    }

    public async Task SaveMovieSearch(string movieTitle, string imdbMovieId)
    {
        var existingSearchQuery = await MovieSearches.FirstOrDefaultAsync(x => x.MovieTitle == movieTitle);

        if (existingSearchQuery is not null)
        {
            return;
        }

        if (await MovieSearches.CountAsync() == MaxSearchCount)
        {
            var savedMovieSearches = await MovieSearches
                .OrderByDescending(x => x.TimeStamp)
                .ToListAsync();
            
            var oldestSearch = savedMovieSearches.LastOrDefault();

            if (oldestSearch is not null)
            {
                MovieSearches.Remove(oldestSearch);
            }
        }
        
        await MovieSearches.AddAsync(new MovieSearchQuery { MovieTitle = movieTitle, ImdbMovieId = imdbMovieId});
        
        await dataContext.SaveChangesAsync();
    }
}

public interface IMovieSearchStorageService
{
    Task SaveMovieSearch(string movieTitle, string imdbMovieId);

    Task<List<SavedSearchDto>> GetRecentSearches();
}