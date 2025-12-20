using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Extensions;

public static class CollectQueryExtensions
{
    public static IQueryable<Collect> ApplyFilters(this IQueryable<Collect> query, CollectFilterViewModel filters)
    {
        if (filters.Id > 0)
        {
            query = query.Where(c => c.Id == filters.Id);
        }

        if (filters.StartCreation.HasValue && filters.EndCreation.HasValue)
        {
            query = query.Where(c => c.CreatedAt >= filters.StartCreation.Value && c.CreatedAt < filters.EndCreation.Value.AddDays(1));
        }

        if (!string.IsNullOrEmpty(filters.User))
        {
            query = query.Where(c => c.User.FullName.Contains(filters.User));
        }

        if (!string.IsNullOrEmpty(filters.Supplier))
        {
            query = query.Where(c => c.Supplier.Name.Contains(filters.Supplier));
        }

        if (filters.StartCollect.HasValue && filters.EndCollect.HasValue)
        {
            query = query.Where(c => c.CollectAt >= filters.StartCollect.Value && c.CollectAt < filters.EndCollect.Value.AddDays(1));
        }

        if (!string.IsNullOrEmpty(filters.Product))
        {
            query = query.Where(c => c.Product.Name.Contains(filters.Product));
        }

        if (filters.Status.HasValue)
        {
            query = query.Where(c => c.Status == filters.Status);
        }

        if (filters.Volume > 0)
        {
            query = query.Where(c => c.Volume == filters.Volume);
        }

        if (filters.Weight > 0)
        {
            query = query.Where(c => c.Weight == filters.Weight);
        }

        if (!string.IsNullOrEmpty(filters.Filial))
        {
            query = query.Where(c => c.Filial.Name.Contains(filters.Filial));
        }

        return query;
    }
}