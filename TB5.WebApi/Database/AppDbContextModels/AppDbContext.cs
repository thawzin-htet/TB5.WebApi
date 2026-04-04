using Microsoft.EntityFrameworkCore;
using TB5.WebApi.Models;

namespace TB5.WebApi.Database.AppDbContextModels
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // For a simple demo, using In-Memory database if not already configured.
            // In a real app, this would use a connection string from appsettings.json.
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("TB5_WebApi_Db");
            }
        }

        public DbSet<TblProduct> TblProducts { get; set; }
    }
}
