using CollectApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(List<ApplicationUser> items, int totalCount)> ToUserListAsync(int pageNum = 1, int pageSize = 10)
        {
            IQueryable<ApplicationUser> query = _userManager.Users.AsQueryable();

            int totalCount = await query.CountAsync();

            List<ApplicationUser> items = await query
                .OrderBy(u => u.Role)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}