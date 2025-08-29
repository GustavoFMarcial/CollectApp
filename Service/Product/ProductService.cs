using CollectApp.Data;
using CollectApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Services
{
    public class ProductService : IProductService
    {
        private readonly CollectAppContext _context;

        public ProductService(CollectAppContext context)
        {
            _context = context;
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public async Task<Product?> FindProductAsync(int? id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<List<Product>> GetAllProductsListAsycn()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<int> SaveChangesProductsAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }
    }
}