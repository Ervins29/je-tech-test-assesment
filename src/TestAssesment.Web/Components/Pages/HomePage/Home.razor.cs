using Microsoft.AspNetCore.Components.Web;
using TestAssesment.Integrations.Omdb.Interfaces;
using TestAssesment.Integrations.Omdb.Models;

namespace TestAssesment.Web.Components.Pages.HomePage;

public partial class Home(IOmdbClient omdbClient, ILogger<Home> logger)
{
    private const int MinSearchLength = 3;

    public string SearchText { get; set; }

    public OmdbMovie? SearchResult { get; set; }

    public async Task SearchMovies(string searchText)
    {
        searchText = searchText.Trim();

        SearchText = searchText;

        logger.LogInformation($"Searching for {SearchText}");

        if (searchText.Length < MinSearchLength)
        {
            SearchResult = null;
            return;
        }

        var searchResult = await omdbClient.SearchMovies(title: searchText);

        SearchResult = searchResult;

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
}