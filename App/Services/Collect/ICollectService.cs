using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Services;

public interface ICollectService
{
    public Task<PagedResultViewModel<CollectListViewModel, CollectFilterViewModel>> SetPagedResultCollectListViewModel(CollectFilterViewModel filters, int pageNum = 1, int pageSize = 10);
    public Task<Collect?> FindCollectAsync(int id);
    public Task CreateCollect(CreateCollectViewModel collectCreate, string userId);
    public Task<EditCollectViewModel?> SetEditCollectViewModel(int id);
    public Task EditCollect(EditCollectViewModel collectEdit);
    public Task UpdateCollectStatus(ChangeCollectViewModel changeStatus);
    public Task CancelCollect(int id);
    public Task<bool> MustBeCollectOwner();
}