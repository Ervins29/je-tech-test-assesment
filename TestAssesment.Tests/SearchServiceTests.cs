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
        ImdbId = "123456789"
    };

    public SearchServiceTests()
    {
        _omdbClientMock
            .Setup(x => x.SearchMovies(_expectedMovie.Title))
            .ReturnsAsync(_expectedMovie);

        _omdbClientMock
            .Setup(x => x.SearchMovies(It.Is<string>(y => y != _expectedMovie.Title)))
            .ReturnsAsync(new OmdbMovie { Response = false });

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
            x => x.SaveMovieSearch(testMovie, "123456789"),
            Times.Once);
    }

    [Fact]
    public async Task SearchMovie_WhenApiReturnsFailure_DoesNotSaveSearchHistory()
    {
        var testMovie = "Test Movie 2";

        var result = await _searchService.SearchMovie(testMovie);

        result.Response.ShouldBeFalse();

        _movieSearchStorageServiceMock.Verify(
            x => x.SaveMovieSearch(testMovie, "1234"),
            Times.Never);
    }
}