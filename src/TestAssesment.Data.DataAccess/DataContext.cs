using Microsoft.EntityFrameworkCore;
using TestAssesment.Data.DataAccess.Models;

namespace TestAssesment.Data.DataAccess;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<MovieSearchQuery> MovieSearchQueries { get; set; }
}