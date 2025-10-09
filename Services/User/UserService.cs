using CollectApp.Models;
using CollectApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<PagedResultViewModel<UserListViewModel>> SetPagedResultUserListViewModel(int pageNum = 1, int pageSize = 10)
        {
            int totalCount = await _userManager.Users.CountAsync();
            List<ApplicationUser> userList = await _userManager.Users
                .OrderBy(u => u.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var users = userList.Select(u => new UserListViewModel
            {
                Id = u.Id,
                FullName = u.FullName,
                Role = u.Role,
            }).ToList();

            return new PagedResultViewModel<UserListViewModel>
            {
                Items = users,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                PageNum = pageNum,
            };
        }
    }
}