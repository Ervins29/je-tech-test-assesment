namespace TestAssesment.Integrations.Omdb.Configurations;

public class OmdbConfiguration
{
    public const string SectionName = "OmdbApi";
    
    public required string BaseUrl { get; set; }

    public required string ApiKey { get; set; }
}