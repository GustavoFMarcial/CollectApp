using CollectApp.Data;
using CollectApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Repositories
{
    public class ProductRepository : IProductRepository
    {
        CollectAppContext _context;

        public ProductRepository(CollectAppContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetProductByIdAsync(int? id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<List<Product>> ToProductListAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<bool> AnyProductAsync(string productDescription, int? productId)
        {
            return await _context.Products.AnyAsync(p => p.Description == productDescription && p.Id != productId);
        }

        public async Task<List<Product>> WhereProductAsync(string input)
        {
            return await _context.Products.Where(p => p.Description.Contains(input)).ToListAsync();
        }

        public async Task AddProduct(Product product)
        {
            _context.Products.Add(product);
            await SaveChangesProductAsync();
        }

        public async Task RemoveProduct(Product product)
        {
            _context.Products.Remove(product);
            await SaveChangesProductAsync();
        }

        public async Task SaveChangesProductAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}