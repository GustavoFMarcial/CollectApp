using CollectApp.Models;
using CollectApp.ViewModels;
using Microsoft.CodeAnalysis.Differencing;

namespace CollectApp.Services
{
    public interface ICollectService
    {
        public Task<PagedResultViewModel<CollectListViewModel>> SetPagedResultCollectListViewModel(int pageNum = 1, int pageSize = 10);
        public Task<Collect?> FindCollectAsync(int? id);
        public Task CreateCollect(CreateCollectViewModel collectCreate, string userId);
        public Task<EditCollectViewModel> SetEditCollectViewModel(int? id);
        public Task EditCollect(EditCollectViewModel collectEdit);
        public Task UpdateCollectStatus(ChangeStatusCollectViewModel changeStatus);
        public Task DeleteCollect(int? id);
    }
}