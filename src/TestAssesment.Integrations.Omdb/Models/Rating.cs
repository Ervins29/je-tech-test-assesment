namespace TestAssesment.Integrations.Omdb.Models;

public record Rating
{
    public string Source { get; set; }
    
    public string Value { get; set; }
}