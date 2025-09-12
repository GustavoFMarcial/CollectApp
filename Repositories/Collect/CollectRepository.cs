// using CollectApp.Data;
// using Microsoft.EntityFrameworkCore;
// using CollectApp.Models;
// using CollectApp.ViewModels;
// using SQLitePCL;
// using Microsoft.CodeAnalysis.CSharp.Syntax;

// namespace CollectApp.Repositories
// {
//     public class CollectRepository : ICollectRepository
//     {
//         private readonly CollectAppContext _context;
//         private readonly ILogger<CollectRepository> _logger;

//         public CollectRepository(CollectAppContext context, ILogger<CollectRepository> logger)
//         {
//             _context = context;
//             _logger = logger;
//         }

//         public async Task<List<Collect>> GetAllCollectsListAsycn()
//         {
//             return await _context.Collects.Include(c => c.Supplier).Include(c => c.Product).Include(c => c.Filial).ToListAsync();
//         }

//         public async Task AddCollect(CreateCollectViewModel collectCreate)
//         {

//             Supplier? supplier = await _context.Collects.FindAsync(collectCreate.SupplierId);
//             Product? product = await _productService.FindProductAsync(collectCreate.ProductId);
//             Filial? filial = await _filialService.FindFilialAsync(collectCreate.FilialId);

//             if (supplier == null || product == null || filial == null)
//             {
//                 return;
//             }

//             Collect collect = new Collect
//             {
//                 SupplierId = supplier.Id,
//                 Supplier = supplier,
//                 CollectAt = collectCreate.CollectAt,
//                 ProductId = product.Id,
//                 Product = product,
//                 Volume = collectCreate.Volume,
//                 Weigth = collectCreate.Weight,
//                 FilialId = filial.Id,
//                 Filial = filial,
//             };

//             _context.Collects.Add(collect);
//             await SaveChangesCollectsAsync();
//         }

//         public async Task EditCollect(EditCollectViewModel collectEdit)
//         {
//             Collect? collect = await FindCollectAsync(collectEdit.Id);

//             if (collect == null)
//             {
//                 return;
//             }

//             Supplier? supplier = await _supplierService.FindSupplierAsync(collectEdit.SupplierId);
//             Product? product = await _productService.FindProductAsync(collectEdit.ProductId);
//             Filial? filial = await _filialService.FindFilialAsync(collectEdit.FilialId);

//             if (supplier == null || product == null || filial == null)
//             {
//                 return;
//             }

//             collect.SupplierId = supplier.Id;
//             collect.Supplier = supplier;
//             collect.CollectAt = collectEdit.CollectAt;
//             collect.ProductId = product.Id;
//             collect.Product = product;
//             collect.Volume = collectEdit.Volume;
//             collect.Weigth = collectEdit.Weight;
//             collect.FilialId = filial.Id;
//             collect.Filial = filial;

//             await SaveChangesCollectsAsync();
//         }

//         public async Task<int> SaveChangesCollectsAsync()
//         {
//             return await _context.SaveChangesAsync();
//         }

//         public async Task<Collect?> FindCollectAsync(int? id)
//         {
//             return await _context.Collects
//                 .Include(c => c.Supplier)
//                 .Include(c => c.Product)
//                 .Include(c => c.Filial)
//                 .FirstOrDefaultAsync(c => c.Id == id);
//         }

//         public async Task UpdateCollectStatus(ChangeStatusCollectViewModel changeStatus)
//         {
//             Collect? collect = await FindCollectAsync(changeStatus.Id);

//             if (collect == null)
//             {
//                 return;
//             }

//             if (changeStatus.Status == null)
//             {
//                 return;
//             }

//             collect.Status = changeStatus.Status;
//             await SaveChangesCollectsAsync();
//         }

//         public async Task DeleteCollect(int? id)
//         {
//             Collect? collect = await FindCollectAsync(id);

//             if (collect == null)
//             {
//                 return;
//             }

//             _context.Collects.Remove(collect);
//             await SaveChangesCollectsAsync();
//         }
//     }
// }