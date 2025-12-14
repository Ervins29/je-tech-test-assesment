using System.Globalization;
using Microsoft.AspNetCore.Components;
using TestAssesment.Integrations.Omdb.Models;

namespace TestAssesment.Web.Components.Pages.MovieDetailsPage.Components;

public partial class HeroBlock
{
    [Parameter]
    public OmdbMovie Movie { get; set; }

    private int GetStarRating(string imdbRating)
    {
        if (double.TryParse(imdbRating, NumberStyles.Float, CultureInfo.InvariantCulture, out var rating))
        {
            return (int)Math.Round(rating / 2);
        }

        return 0;
    }
}