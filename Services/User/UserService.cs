using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Services
{
    public class UserService : IUserService
    {
        // private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<PagedResultViewModel<UserListViewModel>> SetPagedResultUserListViewModel(int pageNum = 1, int pageSize = 10)
        {
            (List<ApplicationUser> items, int totalCount) users = await _userRepository.ToUserListAsync(pageNum);

            var userListViewModel = users.items.Select(u => new UserListViewModel
            {
                Id = u.Id,
                FullName = u.FullName,
                Role = u.Role,
                Status = u.Status,
            }).ToList();

            return new PagedResultViewModel<UserListViewModel>
            {
                Items = userListViewModel,
                TotalPages = (int)Math.Ceiling(users.totalCount / (double)pageSize),
                PageNum = pageNum,
            };
        }

        public async Task ChangeUserStatus(string? id)
        {
            if (id == null)
            {
                return;
            }

            ApplicationUser user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return;
            }

            user.Status = user.Status switch
            {
                UserStatus.Ativo => UserStatus.Inativo,
                UserStatus.Inativo => UserStatus.Ativo,
                _ => user.Status,
            };

            if (user.Status == UserStatus.Ativo)
            {
                await _userRepository.UnlockOutUserAsync(user);
            }
            else
            {
                await _userRepository.LockOutUserAsync(user);
            }

            await _userRepository.SaveChangesUserAsync(user);
        }
    }
}