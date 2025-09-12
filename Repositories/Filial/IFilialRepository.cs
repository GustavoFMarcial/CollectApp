using CollectApp.Models;

namespace CollectApp.Repositories
{
    public interface IFilialRepository
    {
        public Task<Filial?> GetFilialByIdAsync(int? id);
        public Task<List<Filial>> ToFilialListAsync();
        public Task<bool> AnyFilialAsync(string filialName, int? filialId);
        public Task<List<Filial>> WhereFilialAsync(string input);
        public Task AddFilial(Filial filial);
        public Task RemoveFilial(Filial filial);
        public Task SaveChangesFilialAsync();
    }
}