using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Extensions;

public static class UserQueryExtensions
{
    public static IQueryable<ApplicationUser> ApplyFilters(this IQueryable<ApplicationUser> query, UserFilterViewModel filters)
    {
        if (!string.IsNullOrEmpty(filters.Id))
        {
            query = query.Where(u => u.Id == filters.Id);
        }

        if (filters.StartCreation.HasValue && filters.EndCreation.HasValue)
        {
            query = query.Where(u => filters.StartCreation >= u.CreatedAt && u.CreatedAt <= filters.EndCreation);
        }

        if (!string.IsNullOrEmpty(filters.FullName))
        {
            query = query.Where(u => u.FullName.Contains(filters.FullName));
        }

        if (!string.IsNullOrEmpty(filters.Role))
        {
            query = query.Where(u => u.Role == filters.Role);
        }

        return query;
    }
}