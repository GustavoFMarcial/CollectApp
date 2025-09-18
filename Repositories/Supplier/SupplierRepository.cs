using Azure;
using CollectApp.Data;
using CollectApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly CollectAppContext _context;

        public SupplierRepository(CollectAppContext context)
        {
            _context = context;
        }

        public async Task<Supplier?> GetSupplierByIdAsync(int? id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<(List<Supplier> items, int totalCount)> ToSupplierListAsync(int pageNum = 1, int pageSize = 10)
        {
            int totalCount = await _context.Suppliers.CountAsync();

            List<Supplier> items = await _context.Suppliers
                .OrderBy(s => s.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> AnySupplierAsync(string supplierCNPJ, int? supplierId)
        {
            return await _context.Suppliers.AnyAsync(s => s.CNPJ == supplierCNPJ && s.Id != supplierId);
        }

        public async Task<List<Supplier>> WhereSupplierAsync(string input)
        {
            return await _context.Suppliers.Where(s => s.Name.Contains(input)).ToListAsync();
        }

        public void AddSupplier(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
        }

        public void RemoveSupplier(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
        }

        public async Task SaveChangesSupplierAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}