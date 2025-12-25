using CollectApp.Data;
using CollectApp.Dtos;
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

    public async Task<List<Collect>> ToCollectListAsync(CollectFilterViewModel filters)
    {
        IQueryable<Collect> query = _context.Collects.AsQueryable();

        query = query
            .Include(c => c.Supplier)
            .Include(c => c.Product)
            .Include(c => c.Filial)
            .Include(c => c.User)
            .ApplyFilters(filters);

        List<Collect> items = await query.ToListAsync();

        int totalCount = await query.CountAsync();

        return items;
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

    public async Task<int> GetTotalCollects(DateTime startDate, DateTime endDate)
    {
        IQueryable<Collect> query = _context.Collects.AsQueryable();

        int totalCollects = await query
            .Where(c => c.CollectAt >= startDate && c.CollectAt < endDate.AddDays(1))
            .CountAsync();

        return totalCollects;
    }

    public async Task<int> GetTotalVolume(DateTime startDate, DateTime endDate)
    {
        IQueryable<Collect> query = _context.Collects.AsQueryable();

        int totalVolume = await query
            .Where(c => c.CollectAt >= startDate && c.CollectAt < endDate.AddDays(1))
            .SumAsync(c => c.Volume ?? 0);

        return totalVolume;
    }

    public async Task<int> GetTotalWeight(DateTime startDate, DateTime endDate)
    {
        IQueryable<Collect> query = _context.Collects.AsQueryable();

        int totalWeight = await query
            .Where(c => c.CollectAt >= startDate && c.CollectAt < endDate.AddDays(1))
            .SumAsync(c => c.Weight ?? 0);

        return totalWeight;
    }

    public async Task<List<CollectPerStatusDto>> GetCollectsPerStatus(DateTime startDate, DateTime endDate)
    {
        IQueryable<Collect> query = _context.Collects.AsQueryable();

        List<CollectPerStatusDto> collectPerStatusDtoList = await query
            .Where(c => c.CollectAt >= startDate && c.CollectAt < endDate.AddDays(1))
            .GroupBy(c => c.Status)
            .Select(g => new CollectPerStatusDto
            {
                Status = g.Key,
                Total = g.Count(),
            })
            .ToListAsync();

        return collectPerStatusDtoList;
    }

    public async Task<List<CollectPerDayDto>> GetCollectsPerDay(DateTime startDate, DateTime endDate)
    {
        IQueryable<Collect> query = _context.Collects.AsQueryable();

        List<CollectPerDayDto> collectPerDayDtoList = await query
            .Where(c => c.CollectAt >= startDate && c.CollectAt < endDate.AddDays(1))
            .GroupBy(c => c.CollectAt)
            .Select(g => new CollectPerDayDto
            {
                Date = g.Key,
                Total = g.Count(),
            })
            .ToListAsync();

        return collectPerDayDtoList;
    }

    public async Task<TopProductDto> GetTopProduct(DateTime startDate, DateTime endDate)
    {
        IQueryable<Collect> query = _context.Collects.AsQueryable();

        var topProduct = await query
            .Where(c => c.CollectAt >= startDate && c.CollectAt < endDate.AddDays(1))
            .GroupBy(c => c.Product.Name)
            .Select(g =>
            new TopProductDto
            {
                Name = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .FirstOrDefaultAsync();


        return topProduct ?? new TopProductDto { Name = "N/A", Count = 0 };
    }

    public async Task<TopSupplierDto> GetTopSupplier(DateTime startDate, DateTime endDate)
    {
        IQueryable<Collect> query = _context.Collects.AsQueryable();

        var topSupplier = await query
            .Where(c => c.CollectAt >= startDate && c.CollectAt < endDate.AddDays(1))
            .GroupBy(c => c.Supplier.Name)
            .Select(g =>
            new TopSupplierDto
            {
                Name = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .FirstOrDefaultAsync();

        return topSupplier ?? new TopSupplierDto { Name = "N/A", Count = 0 };
    }

    public async Task<TopFilialDto> GetTopFilial(DateTime startDate, DateTime endDate)
    {
        IQueryable<Collect> query = _context.Collects.AsQueryable();

        var topFilial = await query
            .Where(c => c.CollectAt >= startDate && c.CollectAt < endDate.AddDays(1))
            .GroupBy(c => c.Filial.Name)
            .Select(g =>
            new TopFilialDto
            {
                Name = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .FirstOrDefaultAsync();

        return topFilial ?? new TopFilialDto { Name = "N/A", Count = 0 };
    }
}