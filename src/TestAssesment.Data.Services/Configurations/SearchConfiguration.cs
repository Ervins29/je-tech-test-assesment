namespace TestAssesment.Data.Services.Configurations;

public class SearchConfiguration
{
    public const string SectionName = "SearchSettings";

    public int MaxSearchCount { get; set; } = 5;

    public int MinSearchLength { get; set; } = 3;
}