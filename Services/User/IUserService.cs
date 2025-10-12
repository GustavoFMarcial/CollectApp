using CollectApp.ViewModels;

namespace CollectApp.Services
{
    public interface IUserService
    {
        public Task<PagedResultViewModel<UserListViewModel>> SetPagedResultUserListViewModel(int pageNum = 1, int pageSize = 10);
        public Task ChangeUserStatus(string id);
        public Task<EditUserViewModel> SetEditCollectViewModel(string id);
        public Task<OperationResult> EditUser(EditUserViewModel userEdit);
    }
}