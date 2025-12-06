using CollectApp.Models;
using CollectApp.ViewModels;
using CollectApp.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CollectApp.Services;

public class CollectService : ICollectService
{
    private readonly ICollectRepository _collectRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IProductRepository _productRepository;
    private readonly IFilialRepository _filialRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;
    private readonly ILogger<CollectService> _logger;

    public CollectService(ICollectRepository collectRepository, ISupplierRepository supplierRepository, IProductRepository productRepository, IFilialRepository filialRepository, UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService, IAuthorizationService authorizationService, ILogger<CollectService> logger)
    {
        _collectRepository = collectRepository;
        _supplierRepository = supplierRepository;
        _productRepository = productRepository;
        _filialRepository = filialRepository;
        _userManager = userManager;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<PagedResultViewModel<CollectListViewModel, CollectFilterViewModel>> SetPagedResultCollectListViewModel(CollectFilterViewModel filters, int pageNum = 1, int pageSize = 10)
    {
        (List<Collect> items, int totalCount) collects = await _collectRepository.ToCollectListAsync(filters, pageNum);

        var collectTasks = collects.items.Select(async c => new CollectListViewModel
        {
            Id = c.Id,
            CreatedAt = c.CreatedAt,
            UserId = c.User.Id,
            FullName = c.User.FullName,
            SupplierName = c.Supplier.Name,
            CollectAt = c.CollectAt,
            ProductDescription = c.Product.Name,
            Status = c.Status,
            Volume = c.Volume,
            Weigth = c.Weight,
            Filial = c.Filial.Name,
            ChangeCollect = new ChangeCollectViewModel
            {
                Id = c.Id,
                Status = c.Status,
                UserId = c.User.Id,
                CanChangeCollectStatus = (await _authorizationService.AuthorizeAsync(_currentUserService.User, "CanChangeCollectStatus")).Succeeded,
                CanEditOpenOrDeleteCollect = (await _authorizationService.AuthorizeAsync(_currentUserService.User, c.User.Id, "MustBeCollectOwner")).Succeeded,
            }
        });

        List<CollectListViewModel> collectListViewModels = (await Task.WhenAll(collectTasks)).ToList();

        return new PagedResultViewModel<CollectListViewModel, CollectFilterViewModel>
        {
            Items = collectListViewModels,
            TotalPages = (int)Math.Ceiling(collects.totalCount / (double)pageSize),
            PageNum = pageNum,
            Filters = filters,
        };
    }

    public async Task CreateCollect(CreateCollectViewModel collectCreate, string userId)
    {
        Supplier? supplier = await _supplierRepository.GetSupplierByIdAsync(collectCreate.SupplierId);
        Product? product = await _productRepository.GetProductByIdAsync(collectCreate.ProductId);
        Filial? filial = await _filialRepository.GetFilialByIdAsync(collectCreate.FilialId);
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);

        if (user == null || supplier == null || product == null || filial == null)
        {
            return;
        }

        Collect collect = new Collect
        {
            UserId = user.Id,
            User = user,
            SupplierId = supplier.Id,
            Supplier = supplier,
            CollectAt = collectCreate.CollectAt,
            ProductId = product.Id,
            Product = product,
            Volume = collectCreate.Volume,
            Weight = collectCreate.Weight,
            FilialId = filial.Id,
            Filial = filial,
        };

        _collectRepository.AddCollect(collect);
        await _collectRepository.SaveChangesCollectAsync();
    }

    public async Task<EditCollectViewModel?> SetEditCollectViewModel(int id)
    {
        Collect? collect = await _collectRepository.GetCollectByIdAsync(id);

        if (collect == null || collect.Supplier == null || collect.Product == null || collect.Filial == null)
        {
            return null;
        }

        EditCollectViewModel ecvm = new EditCollectViewModel
        {
            Id = collect.Id,
            SupplierId = collect.SupplierId,
            Supplier = collect.Supplier.Name,
            CollectAt = collect.CollectAt,
            ProductId = collect.ProductId,
            Product = collect.Product.Name,
            Volume = collect.Volume,
            Weight = collect.Weight,
            FilialId = collect.FilialId,
            Filial = collect.Filial.Name,
        };

        return ecvm;
    }

    public async Task EditCollect(EditCollectViewModel collectEdit)
    {
        Collect? collect = await _collectRepository.GetCollectByIdAsync(collectEdit.Id);

        if (collect == null || collect.Status != CollectStatus.PendenteAprovar)
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
        collect.Weight = collectEdit.Weight;
        collect.FilialId = filial.Id;
        collect.Filial = filial;

        await _collectRepository.SaveChangesCollectAsync();
    }

    public async Task<Collect?> FindCollectAsync(int id)
    {
        Collect? collect = await _collectRepository.GetCollectByIdAsync(id);

        if (collect == null)
        {
            return null;
        }

        return collect;
    }

    public async Task UpdateCollectStatus(ChangeCollectViewModel changeCollect)
    {
        Collect? collect = await _collectRepository.GetCollectByIdAsync(changeCollect.Id);

        if (collect == null || collect.Status == CollectStatus.Coletado || collect.Status == CollectStatus.Cancelado)
        {
            return;
        }

        if (changeCollect.ToOpen)
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

    public async Task CancelCollect(int id)
    {
        Collect? collect = await _collectRepository.GetCollectByIdAsync(id);

        if (collect == null || collect.Status != CollectStatus.PendenteAprovar)
        {
            return;
        }

        collect.Status = CollectStatus.Cancelado;
        await _collectRepository.SaveChangesCollectAsync();
    }

    public async Task<bool> MustBeCollectOwner()
    {
        return (await _authorizationService.AuthorizeAsync(_currentUserService.User, "MustBeCollectOwner")).Succeeded;
    }
}