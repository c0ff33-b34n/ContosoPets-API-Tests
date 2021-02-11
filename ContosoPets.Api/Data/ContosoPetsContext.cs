using ContosoPets.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPets.Api.Data
{
    public class ContosoPetsContext : DbContext
    {
        public ContosoPetsContext(DbContextOptions<ContosoPetsContext> options) 
            : base(options)
        {
        }

        public DbSet<Product> Products { get; init; }
    }
}