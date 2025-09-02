using Microsoft.EntityFrameworkCore;
using CollectApp.Models;

namespace CollectApp.Data
{
    public class CollectAppContext : DbContext
    {
        public CollectAppContext(DbContextOptions<CollectAppContext> options) : base(options)
        { }

        public DbSet<Collect> Collects { get; set; } = default!;
        public DbSet<Supplier> Suppliers { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Filial> Filials { get; set; } = default!;
    }
}