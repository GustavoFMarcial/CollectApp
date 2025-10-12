using CollectApp.Models;
using Microsoft.AspNetCore.Identity;

namespace CollectApp.Repositories
{
    public interface IUserRepository
    {
        public Task<(List<ApplicationUser> items, int totalCount)> ToUserListAsync(int pageNum = 1, int pageSize = 10);
        public Task<ApplicationUser> GetUserByIdAsync(string id);
        public Task<IdentityResult> SaveChangesUserAsync(ApplicationUser user);
        public Task LockOutUserAsync(ApplicationUser user);
        public Task UnlockOutUserAsync(ApplicationUser user);
        public Task<bool> AnyUserAsync(string userFullName, string userId);
        public Task RemoveRolesFromUserAsync(ApplicationUser user, IList<string> roles);
        public Task AddRoleToUserAsync(ApplicationUser user, string role);
        public Task<IList<string>> GetRolesFromUserAsync(ApplicationUser user);
    }
}