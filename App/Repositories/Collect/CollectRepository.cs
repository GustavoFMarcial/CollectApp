using CollectApp.Data;
using CollectApp.Extensions;
using CollectApp.Models;
using CollectApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Repositories;

public class CollectRepository : ICollectRepository
{
    private readonly CollectAppContext _context;

    public CollectRepository(CollectAppContext context)
    {
        _context = context;
    }

    public async Task<Collect?> GetCollectByIdAsync(int? id)
    {
        return await _context.Collects
            .Include(c => c.Supplier)
            .Include(c => c.Product)
            .Include(c => c.Filial)
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<(List<Collect> items, int totalCount)> ToCollectListAsync(CollectFilterViewModel filters, int pageNum = 1, int pageSize = 10)
    {
        IQueryable<Collect> query = _context.Collects.AsQueryable();

        query = query
            .Include(c => c.Supplier)
            .Include(c => c.Product)
            .Include(c => c.Filial)
            .Include(c => c.User)
            .ApplyFilters(filters);

        List<Collect> items = await query
            .OrderBy(c => c.Id)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        int totalCount = await query.CountAsync();

        return (items, totalCount);
    }

    public async Task<bool> AnyCollectAsync(string type, int? id)
    {
        return type switch
        {
            "supplier" => await _context.Collects.AnyAsync(c => c.SupplierId == id),
            "product" => await _context.Collects.AnyAsync(c => c.ProductId == id),
            "filial" => await _context.Collects.AnyAsync(c => c.FilialId == id),
            _ => true,
        };
    }

    public void AddCollect(Collect collect)
    {
        _context.Collects.Add(collect);
    }

    public async Task SaveChangesCollectAsync()
    {
        await _context.SaveChangesAsync();
    }
}