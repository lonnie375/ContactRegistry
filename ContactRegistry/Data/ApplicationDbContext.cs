using ContactRegistry.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactRegistry.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Registry> Registry {get; set;}
    }
}
