using CollectApp.Data;
using CollectApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Repositories
{
    public class CollectRepository : ICollectRepository
    {
        CollectAppContext _context;

        public CollectRepository(CollectAppContext context)
        {
            _context = context;
        }

        public async Task<Collect?> GetCollectByIdAsync(int? id)
        {
            return await _context.Collects.Include(c => c.Supplier).Include(c => c.Product).Include(c => c.Filial).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Collect>> ToCollectListAsync()
        {
            // return await _context.Collects.ToListAsync();
            return await _context.Collects.Include(c => c.Supplier).Include(c => c.Product).Include(c => c.Filial).ToListAsync();
        }

        // public async Task<bool> AnyProductAsync(string productDescription, int? productId)
        // {
        //     return await _context.Products.AnyAsync(p => p.Description == productDescription && p.Id != productId);
        // }

        // public async Task<List<Product>> WhereProductAsync(string input)
        // {
        //     return await _context.Products.Where(p => p.Description.Contains(input)).ToListAsync();
        // }

        public async Task AddCollect(Collect collect)
        {
            _context.Collects.Add(collect);
            await SaveChangesCollectAsync();
        }

        public async Task RemoveCollect(Collect collect)
        {
            _context.Collects.Remove(collect);
            await SaveChangesCollectAsync();
        }

        public async Task SaveChangesCollectAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}