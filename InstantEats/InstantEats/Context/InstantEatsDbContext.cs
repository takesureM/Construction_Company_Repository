using Microsoft.EntityFrameworkCore;

namespace InstantEats.Context
{
    public class InstantEatsDbContext : DbContext
    {
         public InstantEatsDbContext(DbContextOptions<InstantEatsDbContext> options) : base(options)
          {
               Database.EnsureDeleted();
               Database.EnsureCreated();
          }
    }
}