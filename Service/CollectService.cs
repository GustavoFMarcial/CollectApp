using CollectApp.Data;
using Microsoft.EntityFrameworkCore;
using CollectApp.Models;
using CollectApp.ViewModels;

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

        public void CreateCollect(CollectCreateViewModel collect)
        {
            Collect c = new Collect
            {
                Company = collect.Company,
                CollectAt = collect.CollectAt,
            };

            _context.Collects.Add(c);
        }

        public async Task<int> SaveChangesCollectsAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}