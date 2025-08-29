using CollectApp.Models;

namespace CollectApp.Services
{
    public interface IProductService
    {
        public Task<List<Product>> GetAllProductsListAsycn();
        public Task<Product?> FindProductAsync(int? id);
        public void AddProduct(Product product);
        public Task<int> SaveChangesProductsAsync();
        public void DeleteProduct(Product product);
    }
}