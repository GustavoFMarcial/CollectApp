using Microsoft.EntityFrameworkCore;
using CollectApp.Models;

namespace CollectApp.Data
{
    public class CollectAppContext : DbContext
    {
        public CollectAppContext(DbContextOptions<CollectAppContext> options) : base(options)
        { }

        public DbSet<Collect> Collects { get; set; } = default!;
    }
}