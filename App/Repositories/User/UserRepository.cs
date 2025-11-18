using System.Security.Claims;
using CollectApp.Extensions;
using CollectApp.Models;
using CollectApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserRepository(UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _userStore = userStore;
        _signInManager = signInManager;
    }

    public async Task<(List<ApplicationUser> items, int totalCount)> ToUserListAsync(UserFilterViewModel filters, int pageNum = 1, int pageSize = 10)
    {
        IQueryable<ApplicationUser> query = _userManager.Users.AsQueryable();

        List<ApplicationUser> items = await query
            .ApplyFilters(filters)
            .OrderBy(u => u.Role)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        int totalCount = await query.CountAsync();

        return (items, totalCount);
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"Usuário com Id {id} não foi encontrado.");
        }

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

    public async Task<bool> AnyUserAsync(string userFullName, string? userId)
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

    public async Task SetUserNameAsync(ApplicationUser user, string username, CancellationToken cancellationToken)
    {
        await _userStore.SetUserNameAsync(user, username, cancellationToken);
    }

    public async Task CreateUserAsync(ApplicationUser user, string password)
    {
        await _userManager.CreateAsync(user, password);

        var claims = new List<Claim>
        {
            new Claim("FullName", user.FullName),
        };
        await _userManager.AddClaimsAsync(user, claims);
    }

    public async Task SetLockoutEnabledAsync(ApplicationUser user, bool isLockout)
    {
        await _userManager.SetLockoutEnabledAsync(user, isLockout);
    }

    public async Task<SignInResult> LogInUser(LoginViewModel credentials)
    {
        return await _signInManager.PasswordSignInAsync(credentials.Username, credentials.Password, false, lockoutOnFailure: false);
    }

    public async Task LogOut()
    {
        await _signInManager.SignOutAsync();
    }
}