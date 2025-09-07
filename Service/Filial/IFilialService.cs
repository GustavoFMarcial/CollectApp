using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Services
{
    public interface IFilialService
    {
        public Task<List<Filial>> GetAllFilialsListAsycn();
        public Task<List<Filial>> GetFilteredFilialsAsync(string input);
        public Task<Filial?> FindFilialAsync(int? id);
        public Task<OperationResult> AddFilial(Filial filial);
        public Task<int> SaveChangesFilialsAsync();
        public void DeleteFilial(Filial filial);
        public Task<OperationResult> EditFilial(EditFilialViewModel filialEdit);
    }
}