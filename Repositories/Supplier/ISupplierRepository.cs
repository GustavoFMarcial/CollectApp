using CollectApp.Models;

namespace CollectApp.Repositories
{
    public interface ISupplierRepository
    {
        public Task<Supplier?> GetSupplierByIdAsync(int? id);
        public Task<List<Supplier>> ToSupplierListAsync();
        public Task<bool> AnySupplierAsync(string supplierCNPJ, int? supplierId);
        public Task<List<Supplier>> WhereSupplierAsync(string input);
        public Task AddSupplier(Supplier product);
        public Task RemoveSupplier(Supplier product);
        public Task SaveChangesSupplierAsync();
    }
}