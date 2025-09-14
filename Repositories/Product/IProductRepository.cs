using CollectApp.Models;

namespace CollectApp.Repositories
{
    public interface IProductRepository
    {
        public Task<Product?> GetProductByIdAsync(int? id);
        public Task<List<Product>> ToProductListAsync();
        public Task<bool> AnyProductAsync(string productDescription, int? productId);
        public Task<List<Product>> WhereProductAsync(string input);
        public void AddProduct(Product product);
        public void RemoveProduct(Product product);
        public Task SaveChangesProductAsync();
    }
}