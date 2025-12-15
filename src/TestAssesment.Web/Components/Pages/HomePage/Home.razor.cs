using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TestAssesment.Data.Services.Models;
using TestAssesment.Integrations.Omdb.Models;
using TestAssesment.Web.Services;

namespace TestAssesment.Web.Components.Pages.HomePage;

public partial class Home(SearchService searchService, NavigationManager navigationManager, ILogger<Home> logger)
{
    private const int MinSearchLength = 3;

    public string SearchText { get; set; }

    private OmdbMovie? SearchResult { get; set; }

    private List<SavedSearchDto> SavedSearchDtos { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        await GetRecentSearches();
    }

    private async Task SearchMovies(string searchText)
    {
        searchText = searchText.Trim();

        SearchText = searchText;

        logger.LogInformation($"Searching for {SearchText}");

        if (searchText.Length < MinSearchLength)
        {
            SearchResult = null;
            return;
        }

        var searchResult = await searchService.SearchMovie(searchText);

        SearchResult = searchResult;

        await GetRecentSearches();
        
        StateHasChanged();
    }

    public async Task HandleSearch()
    {
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            await SearchMovies(SearchText);
        }
    }

    public async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await HandleSearch();
        }
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