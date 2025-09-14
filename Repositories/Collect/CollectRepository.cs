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

        public async Task<bool> AnyCollectAsync(string type, int? id)
        {
            switch (type)
            {
                case "supplier":
                    return await _context.Collects.AnyAsync(c => c.SupplierId == id);

                case "product":
                    return await _context.Collects.AnyAsync(c => c.ProductId == id);

                case "filial":
                    return await _context.Collects.AnyAsync(c => c.FilialId == id);

                default:
                    return true;
            }
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