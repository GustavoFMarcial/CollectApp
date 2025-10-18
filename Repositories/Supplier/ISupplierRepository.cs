using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Repositories;

public interface ISupplierRepository
{
    public Task<Supplier?> GetSupplierByIdAsync(int? id);
    public Task<(List<Supplier> items, int totalCount)> ToSupplierListAsync(SupplierFilterViewModel filters, int pageNum = 1, int pageSize = 10, string? input = null);
    public Task<bool> AnySupplierAsync(string supplierCNPJ, int? supplierId);
    public void AddSupplier(Supplier product);
    public void RemoveSupplier(Supplier product);
    public Task SaveChangesSupplierAsync();
}