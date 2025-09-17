using CollectApp.Models;

namespace CollectApp.Repositories
{
    public interface IFilialRepository
    {
        public Task<Filial?> GetFilialByIdAsync(int? id);
        public Task<(List<Filial> Items, int TotalCount)> ToFilialListAsync(int pageNum = 1, int pageSize = 10);
        public Task<bool> AnyFilialAsync(string filialName, int? filialId);
        public Task<List<Filial>> WhereFilialAsync(string input);
        public void AddFilial(Filial filial);
        public void RemoveFilial(Filial filial);
        public Task SaveChangesFilialAsync();
    }
}