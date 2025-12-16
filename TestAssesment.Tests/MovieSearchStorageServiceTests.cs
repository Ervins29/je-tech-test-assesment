using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using TestAssesment.Data.DataAccess;
using TestAssesment.Data.DataAccess.Models;
using TestAssesment.Data.Services;
using TestAssesment.Data.Services.Configurations;

namespace TestAssesment.Tests;

public class MovieSearchStorageServiceTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly IOptions<SearchConfiguration> _searchConfig;
    private readonly MovieSearchStorageService _movieSearchStorageService;

    private readonly List<MovieSearchQuery> _seedData =
    [
        new()
        {
            ImdbMovieId = "111",
            MovieTitle = "The Matrix",
            TimeStamp = DateTime.Parse("2025-12-15 18:37:37")
        },
        new()
        {
            ImdbMovieId = "222",
            MovieTitle = "Titanic",
            TimeStamp = DateTime.Parse("2025-12-15 18:00:00")
        },
        new()
        {
            ImdbMovieId = "333",
            MovieTitle = "Test assesment movie",
            TimeStamp = DateTime.Parse("2025-12-15 18:12:00")
        },
        new()
        {
            ImdbMovieId = "444",
            MovieTitle = "Unknown movie",
            TimeStamp = DateTime.Parse("2025-12-14 18:37:37")
        }
    ];

    public MovieSearchStorageServiceTests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        _dataContext = new DataContext(options);

        _dataContext.Database.OpenConnection();
        _dataContext.Database.EnsureCreated();

        _dataContext.Set<MovieSearchQuery>().AddRange(_seedData);
        _dataContext.SaveChanges();

        _searchConfig = Options.Create(new SearchConfiguration
        {
            MaxSearchCount = 5,
            MinSearchLength = 3
        });

        _movieSearchStorageService = new MovieSearchStorageService(_dataContext, _searchConfig);
    }

    public void Dispose()
    {
        _dataContext.Database.CloseConnection();
        _dataContext.Dispose();
    }

    [Fact]
    public async Task MovieSearchStorageService_WhenSaveSearchQuery_ShouldSave()
    {
        await _movieSearchStorageService.SaveMovieSearch("Test Movie", "55555");

        var recentMovies = await _movieSearchStorageService.GetRecentSearches();

        recentMovies.ShouldNotBeNull();

        var savedMovie = recentMovies.Last();

        recentMovies.Count.ShouldBe(5);
        savedMovie?.Title.ShouldBe("Test Movie");
        savedMovie?.ImdbId.ShouldBe("55555");
    }

    [Fact]
    public async Task MovieSearchStorageService_WhenSaveSearchQuery_ShouldNotSaveAgain()
    {
        const string existingMovieTitle = "Unknown movie";
        const string existingMovieId = "444";

        await _movieSearchStorageService.SaveMovieSearch(existingMovieTitle, existingMovieId);

        var recentMovies = await _movieSearchStorageService.GetRecentSearches();

        recentMovies.ShouldNotBeNull();
        recentMovies.Count.ShouldBeLessThanOrEqualTo(4);

        recentMovies
            .Count(x => x is { Title: existingMovieTitle, ImdbId: existingMovieId })
            .ShouldBeLessThanOrEqualTo(1);
    }

    [Fact]
    public async Task MovieSearchStorageService_WhenSaveSearchQuery_ShouldSaveAndOverrideOldest()
    {
        const string oldestMovieTitle = "Unknown movie";

        const string testMovie = "Test Movie";
        const string testMovie2 = "Test Movie2";

        await _movieSearchStorageService.SaveMovieSearch(testMovie, "55555");
        await _movieSearchStorageService.SaveMovieSearch(testMovie2, "66666");

        var recentMovies = await _movieSearchStorageService.GetRecentSearches();

        recentMovies.ShouldNotBeNull();
        recentMovies.Count.ShouldBeLessThanOrEqualTo(5);
        recentMovies.ShouldContain(x => x.Title == testMovie);
        recentMovies.ShouldContain(x => x.Title == testMovie2);
        recentMovies.ShouldNotContain(x => x.Title == oldestMovieTitle);
    }

    [Fact]
    public async Task MovieSearchStorageService_WhenSaveSearchQuery_ShouldReturnCorrectSearch()
    {
        var recentMovies = await _movieSearchStorageService.GetRecentSearches();

        recentMovies.ShouldNotBeNull();
        recentMovies.Count.ShouldBeLessThanOrEqualTo(4);

        recentMovies.ShouldContain(x => x.Title == "The Matrix");
        recentMovies.ShouldContain(x => x.ImdbId == "111");

        recentMovies.ShouldContain(x => x.Title == "Titanic");
        recentMovies.ShouldContain(x => x.ImdbId == "222");

        recentMovies.ShouldContain(x => x.Title == "Test assesment movie");
        recentMovies.ShouldContain(x => x.ImdbId == "333");

        recentMovies.ShouldContain(x => x.Title == "Unknown movie");
        recentMovies.ShouldContain(x => x.ImdbId == "444");
    }

    [Fact]
    public async Task MovieSearchStorageService_WhenSavingDuplicateWithDifferentCase_ShouldNotSave()
    {
        const string movieTitle = "the matrix";
        const string movieId = "111";

        await _movieSearchStorageService.SaveMovieSearch(movieTitle, movieId);

        var recentMovies = await _movieSearchStorageService.GetRecentSearches();

        recentMovies.ShouldNotBeNull();
        recentMovies.Count.ShouldBe(4);

        recentMovies.Count(x => x.Title.Equals("The Matrix", StringComparison.OrdinalIgnoreCase))
            .ShouldBe(1);
    }
}