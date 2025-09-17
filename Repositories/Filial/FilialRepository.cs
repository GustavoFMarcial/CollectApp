using CollectApp.Data;
using CollectApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Repositories
{
    public class FilialRepository : IFilialRepository
    {
        private readonly CollectAppContext _context;

        public FilialRepository(CollectAppContext context)
        {
            _context = context;
        }

        public async Task<Filial?> GetFilialByIdAsync(int? id)
        {
            return await _context.Filials.FindAsync(id);
        }

        public async Task<(List<Filial> Items, int TotalCount)> ToFilialListAsync(int pageNum = 1, int pageSize = 10)
        {
            int totalCount = await _context.Filials.CountAsync();

            List<Filial> items = await _context.Filials
                .OrderBy(f => f.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> AnyFilialAsync(string filialName, int? filialId)
        {
            return await _context.Filials.AnyAsync(f => f.Name == filialName && f.Id != filialId);
        }

        public async Task<List<Filial>> WhereFilialAsync(string input)
        {
            return await _context.Filials.Where(f => f.Name.Contains(input)).ToListAsync();
        }

        public void AddFilial(Filial filial)
        {
            _context.Filials.Add(filial);
        }

        public void RemoveFilial(Filial filial)
        {
            _context.Filials.Remove(filial);
        }

        public async Task SaveChangesFilialAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}