using Microsoft.EntityFrameworkCore;
using TestAssesment.Data.DataAccess;
using TestAssesment.Data.DataAccess.Models;
using TestAssesment.Data.Services.Models;

namespace TestAssesment.Data.Services;

public class MovieSearchStorageService(DataContext dataContext) : IMovieSearchStorageService
{
    private DbSet<MovieSearchQuery> MovieSearches => dataContext.MovieSearchQueries;

    private const int MaxSearchCount = 5;
    
    public async Task<List<SavedSearchDto>> GetRecentSearches()
    {
        var results = await MovieSearches.AsNoTracking().ToListAsync();

        return results.Select(x => new SavedSearchDto(x.MovieTitle, x.ImdbMovieId)).ToList();
    }

    public async Task SaveMovieSearch(string movieTitle, string imdbMovieId)
    {
        var searchExists = await MovieSearches.AnyAsync(x => x.MovieTitle == movieTitle);

        if (searchExists)
        {
            return;
        }

        if (await MovieSearches.CountAsync() == MaxSearchCount)
        {
            var oldestSearch = await MovieSearches
                .OrderByDescending(x => x.TimeStamp)
                .LastOrDefaultAsync();

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