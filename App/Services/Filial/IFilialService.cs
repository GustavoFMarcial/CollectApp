using CollectApp.ViewModels;

namespace CollectApp.Services;

public interface IFilialService
{
    public Task<PagedResultViewModel<FilialListViewModel, FilialFilterViewModel>> SetPagedResultFilialListViewModel(FilialFilterViewModel filters, int pageNum = 1, int pageSize = 10, string? input = null);
    public Task<OperationResult> CreateFilial(CreateFilialViewModel filialCreate);
    public Task<OperationResult?> DeleteFilial(int id);
    public Task<EditFilialViewModel?> SetEditFilialViewModel(int id);
    public Task<OperationResult?> EditFilial(EditFilialViewModel filialEdit);
}