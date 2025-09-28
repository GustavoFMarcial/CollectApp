using CollectApp.Models;
using CollectApp.ViewModels;
using CollectApp.Repositories;

namespace CollectApp.Services
{
    public class CollectService : ICollectService
    {
        private readonly ICollectRepository _collectRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductRepository _productRepository;
        private readonly IFilialRepository _filialRepository;
        private readonly ILogger<CollectService> _logger;

        public CollectService(ICollectRepository collectRepository, ISupplierRepository supplierRepository, IProductRepository productRepository, IFilialRepository filialRepository, ILogger<CollectService> logger)
        {
            _collectRepository = collectRepository;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
            _filialRepository = filialRepository;
            _logger = logger;
        }

        public async Task<PagedResultViewModel<CollectListViewModel>> SetPagedResultCollectListViewModel(int pageNum = 1, int pageSize = 10)
        {
            (List<Collect> items, int totalCount) collects = await _collectRepository.ToCollectListAsync(pageNum);

            List<CollectListViewModel> collectListViewModels = collects.items.Select(c => new CollectListViewModel
            {
                Id = c.Id,
                CreatedAt = c.CreatedAt,
                SupplierName = c.Supplier.Name,
                CollectAt = c.CollectAt,
                ProductDescription = c.Product.Description,
                Status = c.Status,
                Volume = c.Volume,
                Weigth = c.Weigth,
                Filial = c.Filial.Name,
                ChangeStatus = new ChangeStatusCollectViewModel
                {
                    Id = c.Id,
                    Status = c.Status,
                }
            }).ToList();

            PagedResultViewModel<CollectListViewModel> pagedResultCollectListViewModel = new PagedResultViewModel<CollectListViewModel>
            {
                Items = collectListViewModels,
                TotalPages = (int)Math.Ceiling(collects.totalCount / (double)pageSize),
                PageNum = pageNum,
            };

            return pagedResultCollectListViewModel;
        }

        public async Task CreateCollect(CreateCollectViewModel collectCreate)
        {
            Supplier? supplier = await _supplierRepository.GetSupplierByIdAsync(collectCreate.SupplierId);
            Product? product = await _productRepository.GetProductByIdAsync(collectCreate.ProductId);
            Filial? filial = await _filialRepository.GetFilialByIdAsync(collectCreate.FilialId);

            if (supplier == null || product == null || filial == null)
            {
                return;
            }

            Collect collect = new Collect
            {
                SupplierId = supplier.Id,
                Supplier = supplier,
                CollectAt = collectCreate.CollectAt,
                ProductId = product.Id,
                Product = product,
                Volume = collectCreate.Volume,
                Weigth = collectCreate.Weight,
                FilialId = filial.Id,
                Filial = filial,
            };

            _collectRepository.AddCollect(collect);
            await _collectRepository.SaveChangesCollectAsync();
        }

        public async Task<EditCollectViewModel> SetEditCollectViewModel(int? id)
        {
            Collect? collect = await _collectRepository.GetCollectByIdAsync(id);

            if (collect == null || collect.Supplier == null || collect.Product == null || collect.Filial == null)
            {
                EditCollectViewModel NotFound = new EditCollectViewModel();
                return NotFound;
            }

            EditCollectViewModel ecvm = new EditCollectViewModel
            {
                Id = collect.Id,
                SupplierId = collect.SupplierId,
                Supplier = collect.Supplier.Name,
                CollectAt = collect.CollectAt,
                ProductId = collect.ProductId,
                Product = collect.Product.Description,
                Volume = collect.Volume,
                Weight = collect.Weigth,
                FilialId = collect.FilialId,
                Filial = collect.Filial.Name,
            };

            return ecvm;
        }

        public async Task EditCollect(EditCollectViewModel collectEdit)
        {
            Collect? collect = await _collectRepository.GetCollectByIdAsync(collectEdit.Id);

            if (collect == null)
            {
                return;
            }

            Supplier? supplier = await _supplierRepository.GetSupplierByIdAsync(collectEdit.SupplierId);
            Product? product = await _productRepository.GetProductByIdAsync(collectEdit.ProductId);
            Filial? filial = await _filialRepository.GetFilialByIdAsync(collectEdit.FilialId);

            if (supplier == null || product == null || filial == null)
            {
                return;
            }

            collect.SupplierId = supplier.Id;
            collect.Supplier = supplier;
            collect.CollectAt = collectEdit.CollectAt;
            collect.ProductId = product.Id;
            collect.Product = product;
            collect.Volume = collectEdit.Volume;
            collect.Weigth = collectEdit.Weight;
            collect.FilialId = filial.Id;
            collect.Filial = filial;

            await _collectRepository.SaveChangesCollectAsync();
        }

        public async Task<Collect?> FindCollectAsync(int? id)
        {
            return await _collectRepository.GetCollectByIdAsync(id);
        }

        public async Task UpdateCollectStatus(ChangeStatusCollectViewModel changeStatus)
        {
            Collect? collect = await _collectRepository.GetCollectByIdAsync(changeStatus.Id);

            if (collect == null)
            {
                return;
            }

            if (changeStatus.ToOpen)
            {
                collect.Status = CollectStatus.PendenteAprovar;
            }
            else
            {
                collect.Status = collect.Status switch
                {
                    CollectStatus.PendenteAprovar => CollectStatus.PendenteColetar,
                    CollectStatus.PendenteColetar => CollectStatus.Coletado,
                    _ => collect.Status,
                };   
            }

            await _collectRepository.SaveChangesCollectAsync();
        }

        public async Task DeleteCollect(int? id)
        {
            Collect? collect = await _collectRepository.GetCollectByIdAsync(id);

            if (collect == null)
            {
                return;
            }

            _collectRepository.RemoveCollect(collect);
            await _collectRepository.SaveChangesCollectAsync();
        }
    }
}