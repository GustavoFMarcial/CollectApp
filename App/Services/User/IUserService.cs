using CollectApp.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CollectApp.Services;

public interface IUserService
{
    public Task<PagedResultViewModel<UserListViewModel, UserFilterViewModel>> SetPagedResultUserListViewModel(UserFilterViewModel filters, int pageNum = 1, int pageSize = 10);
    public Task ChangeUserStatus(string id);
    public Task<EditUserViewModel?> SetEditUserViewModel(string id);
    public Task<OperationResult> CreateUser(CreateUserViewModel createUser);
    public Task<OperationResult> EditUser(EditUserViewModel userEdit);
    public Task<SignInResult> LogIn(LoginViewModel credentials);
    public Task LogOut();
}
