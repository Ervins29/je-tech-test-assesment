using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TestAssesment.Data.DataAccess;
using TestAssesment.Data.DataAccess.Models;
using TestAssesment.Data.Services.Configurations;
using TestAssesment.Data.Services.Models;

namespace TestAssesment.Data.Services;

public class MovieSearchStorageService(DataContext dataContext, IOptions<SearchConfiguration> searchConfig)
    : IMovieSearchStorageService
{
    private DbSet<MovieSearchQuery> MovieSearches => dataContext.MovieSearchQueries;

    private int MaxSearchCount => searchConfig.Value.MaxSearchCount;

    public async Task<List<SavedSearchDto>> GetRecentSearches()
    {
        var results = await MovieSearches.AsNoTracking().ToListAsync();

        return results.Select(x => new SavedSearchDto(x.MovieTitle, x.ImdbMovieId)).ToList();
    }

    public async Task SaveMovieSearch(string movieTitle, string imdbMovieId)
    {
        var searchExists = await MovieSearches.AnyAsync(x => x.MovieTitle.ToLower() == movieTitle.ToLower());

        if (searchExists) return;

        if (await MovieSearches.CountAsync() == MaxSearchCount)
        {
            var oldestSearch = await MovieSearches
                .OrderBy(x => x.TimeStamp)
                .FirstOrDefaultAsync();

            if (oldestSearch is not null) MovieSearches.Remove(oldestSearch);
        }

        await MovieSearches.AddAsync(new MovieSearchQuery { MovieTitle = movieTitle, ImdbMovieId = imdbMovieId });

        await dataContext.SaveChangesAsync();
    }
}