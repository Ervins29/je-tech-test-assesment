using Microsoft.AspNetCore.Components;
using TestAssesment.Integrations.Omdb.Models;

namespace TestAssesment.Web.Components.Pages.HomePage.Components;

public partial class MovieCard(NavigationManager navigationManager)
{
    [Parameter]
    public OmdbMovie Movie { get; set; }

    [Parameter]
    public bool Loading { get; set; }

    private void RedirectToMovieDetails()
    {
        navigationManager.NavigateTo($"/MovieDetails/{Movie.ImdbId}");
    }
}