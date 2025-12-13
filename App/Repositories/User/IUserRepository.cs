using CollectApp.Models;
using CollectApp.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CollectApp.Repositories;

public interface IUserRepository
{
    public Task<(List<ApplicationUser> items, int totalCount)> ToUserListAsync(UserFilterViewModel filters, int pageNum = 1, int pageSize = 10);
    public Task<ApplicationUser?> GetUserByIdAsync(string id);
    public Task<IdentityResult> SaveChangesUserAsync(ApplicationUser user);
    public Task LockOutUserAsync(ApplicationUser user);
    public Task UnlockOutUserAsync(ApplicationUser user);
    public Task<bool> AnyUserAsync(string userFullName, string? userId);
    public Task RemoveRolesFromUserAsync(ApplicationUser user, IList<string> roles);
    public Task AddRoleToUserAsync(ApplicationUser user, string role);
    public Task<IList<string>> GetRolesFromUserAsync(ApplicationUser user);
    public Task SetUserNameAsync(ApplicationUser user, string username, CancellationToken cancellationToken);
    public Task CreateUserAsync(ApplicationUser user, string password);
    public Task SetLockoutEnabledAsync(ApplicationUser user, bool isLockout);
    public Task<SignInResult> LogInUser(LoginViewModel credentials);
    public Task LogOut();
}