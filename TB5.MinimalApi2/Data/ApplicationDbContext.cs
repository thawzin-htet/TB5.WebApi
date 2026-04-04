using Microsoft.EntityFrameworkCore;
using TB5.MinimalApi2.Models;

namespace TB5.MinimalApi2.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
}
