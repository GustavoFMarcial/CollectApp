using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Extensions
{
    public static class ProductQueryExtensions
    {
        public static IQueryable<Product> ApplyFilters(this IQueryable<Product> query, ProductFilterViewModel filters)
        {
            if (filters.Id > 0)
            {
                query = query.Where(p => p.Id == filters.Id);
            }

            if (!string.IsNullOrEmpty(filters.Description))
            {
                query = query.Where(p => p.Description.Contains(filters.Description));
            }

            return query;
        }
    }
}