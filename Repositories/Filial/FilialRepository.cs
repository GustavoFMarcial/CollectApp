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

        public async Task<List<Filial>> ToFilialListAsync()
        {
            return await _context.Filials.ToListAsync();
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