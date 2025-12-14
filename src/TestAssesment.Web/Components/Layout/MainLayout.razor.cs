using MudBlazor;

namespace TestAssesment.Web.Components.Layout;

public partial class MainLayout
{
    private bool _isDarkMode;

    public MudTheme _theme = new()
    {
        PaletteLight = new PaletteLight
        {
            Background = "#667eea",
            Primary = "#ffffff",
            Secondary = "#000000",
            Surface = "#F2F2F2",
            TextPrimary = "#333333",
            TextSecondary = "#ffffff"
        }
    };
}