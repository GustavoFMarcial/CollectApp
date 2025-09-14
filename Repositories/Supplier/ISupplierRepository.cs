using CollectApp.Models;

namespace CollectApp.Repositories
{
    public interface ISupplierRepository
    {
        public Task<Supplier?> GetSupplierByIdAsync(int? id);
        public Task<List<Supplier>> ToSupplierListAsync();
        public Task<bool> AnySupplierAsync(string supplierCNPJ, int? supplierId);
        public Task<List<Supplier>> WhereSupplierAsync(string input);
        public void AddSupplier(Supplier product);
        public void RemoveSupplier(Supplier product);
        public Task SaveChangesSupplierAsync();
    }
}