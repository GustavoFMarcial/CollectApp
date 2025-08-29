using CollectApp.Data;
using Microsoft.EntityFrameworkCore;
using CollectApp.Models;

namespace CollectApp.Services
{
    public class CollectService : ICollectService
    {
        private readonly CollectAppContext _context;

        public CollectService(CollectAppContext context)
        {
            _context = context;
        }

        public async Task<List<Collect>> GetAllCollectsListAsycn()
        {
            return await _context.Collects.ToListAsync();
        }

        public void AddCollect(Collect collect)
        {
            _context.Collects.Add(collect);
        }

        public async Task<int> SaveChangesCollectsAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Collect?> FindCollectAsync(int? id)
        {
            return await _context.Collects.FindAsync(id);
        }

        public void UpdateCollectStatus(Collect collect)
        {
            _context.Attach(collect);
            _context.Entry(collect).Property(c => c.Status).IsModified = true;
        }

        public void DeleteCollect(Collect collect)
        {
            _context.Collects.Remove(collect);
        }
    }
}