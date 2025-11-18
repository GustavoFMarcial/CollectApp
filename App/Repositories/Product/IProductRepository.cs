using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Repositories;

public interface IProductRepository
{
    public Task<Product?> GetProductByIdAsync(int? id);
    public Task<(List<Product> items, int totalCount)> ToProductListAsync(ProductFilterViewModel filters, int pageNum = 1, int pageSize = 10, string? input = null);
    public Task<bool> AnyProductAsync(string productDescription, int? productId);
    public void AddProduct(Product product);
    public void RemoveProduct(Product product);
    public Task SaveChangesProductAsync();
}