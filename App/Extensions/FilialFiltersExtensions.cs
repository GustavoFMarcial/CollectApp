using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Extensions;

public static class FilialQueryExtensions
{
    public static IQueryable<Filial> ApplyFilters(this IQueryable<Filial> query, FilialFilterViewModel filters)
    {
        if (filters.Id > 0)
        {
            query = query.Where(f => f.Id == filters.Id);
        }

        if (!string.IsNullOrEmpty(filters.Name))
        {
            query = query.Where(f => f.Name.Contains(filters.Name));
        }

        return query;
    }
}