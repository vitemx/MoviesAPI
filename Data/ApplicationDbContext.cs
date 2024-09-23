using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models;

namespace MoviesAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        //Models
        public DbSet<Category> Category { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<User> User { get; set; }
    }
}