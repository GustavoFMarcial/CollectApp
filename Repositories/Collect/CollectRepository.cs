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
            return await _context.Collects.Include(c => c.Supplier).Include(c => c.Product).Include(c => c.Filial).ToListAsync();
        }

        public void AddCollect(Collect collect)
        {
            _context.Collects.Add(collect);
        }

        public void RemoveCollect(Collect collect)
        {
            _context.Collects.Remove(collect);
        }

        public async Task SaveChangesCollectAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}