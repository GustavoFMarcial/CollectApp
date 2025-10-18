using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Repositories;

public interface IFilialRepository
{
    public Task<Filial?> GetFilialByIdAsync(int? id);
    public Task<(List<Filial> Items, int TotalCount)> ToFilialListAsync(FilialFilterViewModel filters, int pageNum = 1, int pageSize = 10, string? input = null);
    public Task<bool> AnyFilialAsync(string filialName, int? filialId);
    public void AddFilial(Filial filial);
    public void RemoveFilial(Filial filial);
    public Task SaveChangesFilialAsync();
}