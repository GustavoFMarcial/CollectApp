using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CollectApp.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<PagedResultViewModel<UserListViewModel, UserFilterViewModel>> SetPagedResultUserListViewModel(UserFilterViewModel filters, int pageNum = 1, int pageSize = 10)
    {
        (List<ApplicationUser> items, int totalCount) users = await _userRepository.ToUserListAsync(filters, pageNum);

        var userListViewModel = users.items.Select(u => new UserListViewModel
        {
            Id = u.Id,
            CreatedAt = u.CreatedAt,
            FullName = u.FullName,
            Role = u.Role,
            Status = u.Status,
        }).ToList();

        return new PagedResultViewModel<UserListViewModel, UserFilterViewModel>
        {
            Items = userListViewModel,
            TotalPages = (int)Math.Ceiling(users.totalCount / (double)pageSize),
            PageNum = pageNum,
            Filters = filters,
        };
    }

    public async Task ChangeUserStatus(string id)
    {
        ApplicationUser? user = await _userRepository.GetUserByIdAsync(id);

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

    public async Task<OperationResult> CreateUser(CreateUserViewModel createUser)
    {
        bool userExist = await _userRepository.AnyUserAsync(createUser.FullName, null);

        if (userExist)
        {
            return OperationResult.Fail("Já existe um usuário com o nome completo informado.");
        }

        var user = new ApplicationUser
        {
            FullName = createUser.FullName,
            Role = createUser.Role,
            UserName = createUser.Username,
        };

        await _userRepository.SetUserNameAsync(user, createUser.Username, CancellationToken.None);
        await _userRepository.CreateUserAsync(user, createUser.Password);
        await _userRepository.SetLockoutEnabledAsync(user, true);
        await _userRepository.AddRoleToUserAsync(user, createUser.Role);

        return OperationResult.Ok();
    }

    public async Task<EditUserViewModel?> SetEditUserViewModel(string id)
    {
        ApplicationUser? user = await _userRepository.GetUserByIdAsync(id);

        if (user == null)
        {
            return null;
        }

        EditUserViewModel euvm = new EditUserViewModel
        {
            FullName = user.FullName,
            Role = user.Role,
        };

        return euvm;
    }

    public async Task<OperationResult> EditUser(EditUserViewModel userEdit)
    {
        var user = await _userRepository.GetUserByIdAsync(userEdit.Id);

        if (user == null)
        {
            return OperationResult.Fail("Usuário não encontrado.");
        }

        bool userExist = await _userRepository.AnyUserAsync(userEdit.FullName, userEdit.Id);

        if (userExist)
        {
            return OperationResult.Fail("Já existe um usuário com o nome completo informado.");
        }

        IList<string> currentRoles = await _userRepository.GetRolesFromUserAsync(user);
        if (currentRoles.Any())
        {
            await _userRepository.RemoveRolesFromUserAsync(user, currentRoles);
        }

        await _userRepository.AddRoleToUserAsync(user, userEdit.Role);

        user.FullName = userEdit.FullName;
        user.Role = userEdit.Role;

        await _userRepository.SaveChangesUserAsync(user);

        return OperationResult.Ok();
    }

    public async Task<SignInResult> LogIn(LoginViewModel credentials)
    {
        return await _userRepository.LogInUser(credentials);
    }

    public async Task LogOut()
    {
        await _userRepository.LogOut();
    }
}