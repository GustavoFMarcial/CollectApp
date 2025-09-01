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
            return await _context.Collects.Include(c => c.Supplier).Include(c => c.Product).ToListAsync();
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

        public async Task<Supplier?> FindSupplierAsync(int? id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<Product?> FindProductAsync(int? id)
        {
            return await _context.Products.FindAsync(id);
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

        public async Task<List<Product>> GetFilteredProductsAsync(string input)
        {
            return await _context.Products.Where(p => p.Description.Contains(input)).ToListAsync();
        }

        public async Task<List<Supplier>> GetFilteredSuppliersAsync(string input)
        {
            return await _context.Suppliers.Where(s => s.Name.Contains(input)).ToListAsync();
        }

        public async Task<List<Supplier>> GetRegisteredSuppliersAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }
    }
}