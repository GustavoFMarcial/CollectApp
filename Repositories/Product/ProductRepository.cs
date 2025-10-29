using CollectApp.Data;
using CollectApp.Extensions;
using CollectApp.Models;
using CollectApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly CollectAppContext _context;

    public ProductRepository(CollectAppContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetProductByIdAsync(int? id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<(List<Product> items, int totalCount)> ToProductListAsync(ProductFilterViewModel filters, int pageNum = 1, int pageSize = 10, string? input = null)
    {
        IQueryable<Product> query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(input))
        {
            query = query
                .Where(p => p.Name.Contains(input));
        }

        List<Product> items = await query
            .ApplyFilters(filters)
            .OrderBy(p => p.Id)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        int totalCount = await query.CountAsync();

        return (items, totalCount);
    }

    public async Task<bool> AnyProductAsync(string productDescription, int? productId)
    {
        return await _context.Products.AnyAsync(p => p.Name == productDescription && p.Id != productId);
    }

    public void AddProduct(Product product)
    {
        _context.Products.Add(product);
    }

    public void RemoveProduct(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task SaveChangesProductAsync()
    {
        await _context.SaveChangesAsync();
    }
}