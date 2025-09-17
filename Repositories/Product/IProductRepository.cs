using CollectApp.Models;

namespace CollectApp.Repositories
{
    public interface IProductRepository
    {
        public Task<Product?> GetProductByIdAsync(int? id);
        public Task<(List<Product> items, int totalCount)> ToProductListAsync(int pageNum = 1, int pageSize = 10);
        public Task<bool> AnyProductAsync(string productDescription, int? productId);
        public Task<List<Product>> WhereProductAsync(string input);
        public void AddProduct(Product product);
        public void RemoveProduct(Product product);
        public Task SaveChangesProductAsync();
    }
}