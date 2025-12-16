using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;
using TestAssesment.Data.Services;
using TestAssesment.Integrations.Omdb.Interfaces;
using TestAssesment.Integrations.Omdb.Models;
using TestAssesment.Services.Services;

namespace TestAssesment.Tests;

public class SearchServiceTests
{
    private readonly Mock<IOmdbClient> _omdbClientMock = new();
    private readonly Mock<IMovieSearchStorageService> _movieSearchStorageServiceMock = new();
    private readonly ILogger<SearchService> _logger = NullLogger<SearchService>.Instance;

    private readonly SearchService _searchService;

    private readonly OmdbMovie _expectedMovie = new()
    {
        Response = true,
        Title = "Test Movie",
        ImdbId = "tt1234567"
    };

    public SearchServiceTests()
    {
        _omdbClientMock
            .Setup(x => x.SearchMovies("Test Movie", null))
            .ReturnsAsync(_expectedMovie);

        _omdbClientMock
            .Setup(x => x.SearchMovies(It.Is<string>(y => y != "Test Movie"), null))
            .ReturnsAsync(new OmdbMovie { Response = false, Error = "Movie not found!" });

        _searchService = new SearchService(_omdbClientMock.Object, _movieSearchStorageServiceMock.Object, _logger);
    }

    [Fact]
    public async Task SearchMovie_WhenApiReturnsSuccess_SavesSearchHistory()
    {
        var testMovie = "Test Movie";

        var result = await _searchService.SearchMovie(testMovie);

        result.Response.ShouldBeTrue();
        result.Title.ShouldBe(_expectedMovie.Title);

        _movieSearchStorageServiceMock.Verify(
            x => x.SaveMovieSearch(testMovie, "tt1234567"),
            Times.Once);
    }

    [Fact]
    public async Task SearchMovie_WhenApiReturnsFailure_DoesNotSaveSearchHistory()
    {
        var testMovie = "Test Movie 2";

        var result = await _searchService.SearchMovie(testMovie);

        result.Response.ShouldBeFalse();

        _movieSearchStorageServiceMock.Verify(
            x => x.SaveMovieSearch(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task SearchMovie_WhenApiReturnsIncompleteData_ReturnsError()
    {
        var incompleteMovie = new OmdbMovie
        {
            Response = true,
            Title = null,
            ImdbId = "tt1234567"
        };

        _omdbClientMock
            .Setup(x => x.SearchMovies("Incomplete Movie", null))
            .ReturnsAsync(incompleteMovie);

        var result = await _searchService.SearchMovie("Incomplete Movie");

        result.Response.ShouldBeFalse();
        result.Error.ShouldBe("Incomplete movie data returned from API");

        _movieSearchStorageServiceMock.Verify(
            x => x.SaveMovieSearch(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }
}