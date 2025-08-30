using System.Threading.Tasks;
using CollectApp.Data;
using CollectApp.Models;
using CollectApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly CollectAppContext _context;

        public SupplierService(CollectAppContext context)
        {
            _context = context;
        }

        public async Task<List<Supplier>> GetAllSuppliersListAsycn()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<OperationResult> AddSupplier(Supplier supplier)
        {
            bool supplierExist = await _context.Suppliers.AnyAsync(s => s.CNPJ == supplier.CNPJ && s.Id != supplier.Id);

            if (supplierExist)
            {
                return OperationResult.Fail("Já existe um fornecedor cadastrado com o CNPJ fornecido");
            }

            _context.Suppliers.Add(supplier);
            return OperationResult.Ok();
        }

        public async Task<OperationResult> EditSupplier(EditSupplierViewModel supplierEdit)
        {
            bool supplierExist = await _context.Suppliers.AnyAsync(s => s.CNPJ == supplierEdit.CNPJ && s.Id != supplierEdit.Id);

            if (supplierExist)
            {
                return OperationResult.Fail("Já existe um fornecedor cadastrado com o CNPJ fornecido");
            }

            return OperationResult.Ok();
        }

        public async Task<int> SaveChangesSuppliersAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Supplier?> FindSupplierAsync(int? id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public void DeleteSupplier(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
        }
    }
}