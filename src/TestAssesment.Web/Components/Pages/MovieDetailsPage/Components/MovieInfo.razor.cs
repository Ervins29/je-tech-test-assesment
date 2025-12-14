using TestAssesment.Integrations.Omdb.Models;

namespace TestAssesment.Web.Components.Pages.MovieDetailsPage.Components;

public partial class MovieInfo
{
    public OmdbMovie Movie { get; set; }

    private string GetInitials(string name)
    {
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2)
        {
            return $"{parts[0][0]}{parts[parts.Length - 1][0]}".ToUpper();
        }

        return parts.Length > 0 ? parts[0][0].ToString().ToUpper() : "?";
    }
}