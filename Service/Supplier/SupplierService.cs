using CollectApp.Data;
using CollectApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly CollectAppContext _context;

        public SupplierService(CollectAppContext context)
        {
            _context = context;
        }

        public async Task<List<Supplier>> GetAllSuppliersListAsycn()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public void AddSupplier(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
        }

        public async Task<int> SaveChangesSuppliersAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Supplier?> FindSupplierAsync(int? id)
        {
            return await _context.Suppliers.FindAsync(id);
        }
    }
}