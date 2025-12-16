using System.ComponentModel.DataAnnotations;

namespace TestAssesment.Data.DataAccess.Models;

public class MovieSearchQuery
{
    public int Id { get; set; }

    [MaxLength(50)]
    public required string MovieTitle { get; set; }

    [MaxLength(10)]
    public required string ImdbMovieId { get; set; }

    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
}