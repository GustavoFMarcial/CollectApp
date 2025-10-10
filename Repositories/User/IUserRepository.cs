using CollectApp.Models;

namespace CollectApp.Repositories
{
    public interface IUserRepository
    {
        public Task<(List<ApplicationUser> items, int totalCount)> ToUserListAsync(int pageNum = 1, int pageSize = 10);
    }
}