using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Services
{
    public interface IFilialService
    {
        public Task<(List<Filial>, int)> GetAllFilialsListAsycn();
        public Task<List<Filial>> GetFilteredFilialsAsync(string input);
        public Task<PagedResult<FilialListViewModel>> SetFilialListViewModel(int pageNum = 1, int pageSize = 10);
        public Task<OperationResult> CreateFilial(CreateFilialViewModel filialCreate);
        public Task<OperationResult> DeleteFilial(int? id);
        public Task<OperationResult> EditFilial(EditFilialViewModel filialEdit);
        public Task<EditFilialViewModel> SetEditFilialViewModel(int? id);
    }
}