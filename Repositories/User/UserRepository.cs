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

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"Usuário com Id {id} não foi encontrado.");

            return user;
        }

        public async Task<IdentityResult> SaveChangesUserAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task LockOutUserAsync(ApplicationUser user)
        {
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            await _userManager.UpdateSecurityStampAsync(user);
        }

        public async Task UnlockOutUserAsync(ApplicationUser user)
        {
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
        }

        public async Task<bool> AnyUserAsync(string userFullName, string userId)
        {
            return await _userManager.Users.AnyAsync(u => u.FullName == userFullName && u.Id != userId);
        }

        public async Task RemoveRolesFromUserAsync(ApplicationUser user, IList<string> roles)
        {
            await _userManager.RemoveFromRolesAsync(user, roles);
        }

        public async Task AddRoleToUserAsync(ApplicationUser user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IList<string>> GetRolesFromUserAsync(ApplicationUser user)
        {
           return await _userManager.GetRolesAsync(user); 
        }
    }
}