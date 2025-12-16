using Microsoft.AspNetCore.Components;
using TestAssesment.Integrations.Omdb.Interfaces;
using TestAssesment.Integrations.Omdb.Models;

namespace TestAssesment.Web.Components.Pages.MovieDetailsPage;

public partial class MovieDetails(IOmdbClient omdbClient, NavigationManager navigationManager)
{
    [Parameter]
    public string MovieId { get; set; } = string.Empty;

    public OmdbMovie? Movie { get; set; }

    public bool IsLoading { get; set; } = true;

    protected override async Task OnInitializedAsync()
    {
        var movie = await omdbClient.SearchMovies(id: MovieId);

        Movie = movie;

        if (!movie.Response) navigationManager.NavigateTo("/not-found");

        IsLoading = false;
    }

    private string GetInitials(string name)
    {
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2) return $"{parts[0][0]}{parts[parts.Length - 1][0]}".ToUpper();
        return parts.Length > 0 ? parts[0][0].ToString().ToUpper() : "?";
    }
}