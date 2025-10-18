using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Extensions;

public static class SupplierQueryExtensions
{
    public static IQueryable<Supplier> ApplyFilters(this IQueryable<Supplier> query, SupplierFilterViewModel filters)
    {
        if (filters.Id > 0)
        {
            query = query.Where(s => s.Id == filters.Id);
        }

        if (!string.IsNullOrEmpty(filters.Supplier))
        {
            query = query.Where(s => s.Name.Contains(filters.Supplier));
        }

        if (!string.IsNullOrEmpty(filters.CNPJ))
        {
            query = query.Where(s => s.CNPJ == filters.CNPJ);
        }

        if (!string.IsNullOrEmpty(filters.Address))
        {
            query = query.Where(s => s.City.Contains(filters.Address));
        }

        return query;
    }
}