using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;
using TestAssesment.Data.Services.Configurations;
using TestAssesment.Data.Services.Models;
using TestAssesment.Integrations.Omdb.Models;
using TestAssesment.Services.Services;

namespace TestAssesment.Web.Components.Pages.HomePage;

public partial class Home(
    ISearchService searchService,
    NavigationManager navigationManager,
    ILogger<Home> logger,
    IOptions<SearchConfiguration> searchConfig)
{
    private int MinSearchLength => searchConfig.Value.MinSearchLength;

    public string SearchText { get; set; } = string.Empty;

    private OmdbMovie? SearchResult { get; set; }

    private List<SavedSearchDto> SavedSearchDtos { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        await GetRecentSearches();
    }

    private async Task SearchMovies(string searchText)
    {
        searchText = searchText.Trim();

        if (searchText.Length < MinSearchLength)
        {
            SearchResult = null;
            return;
        }

        SearchText = searchText;

        logger.LogInformation("Searching for {SearchText}", SearchText);

        var searchResult = await searchService.SearchMovie(searchText);

        SearchResult = searchResult;

        await GetRecentSearches();

        StateHasChanged();
    }

    public async Task HandleSearch()
    {
        if (!string.IsNullOrWhiteSpace(SearchText)) await SearchMovies(SearchText);
    }

    public async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter") await HandleSearch();
    }

    public void NavigateToMovieDetails(string imdbId)
    {
        navigationManager.NavigateTo($"/MovieDetails/{imdbId}");
    }

    private async Task GetRecentSearches()
    {
        SavedSearchDtos = await searchService.GetRecentSearches();
    }
}