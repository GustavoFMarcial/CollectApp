using Microsoft.AspNetCore.Mvc;
using CollectApp.Models;
using CollectApp.Services;
using CollectApp.ViewModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace CollectApp.Controllers;

public class CollectController : Controller
{
    private readonly ILogger<CollectController> _looger;
    private readonly ICollectService _collectService;

    public CollectController(ILogger<CollectController> logger, ICollectService collectService)
    {
        _looger = logger;
        _collectService = collectService;
    }

    public async Task<IActionResult> ListCollects()
    {
        List<Collect> collects = await _collectService.GetAllCollectsListAsycn();

        List<CollectListItemViewModel> clivm = collects.Select(c => new CollectListItemViewModel
        {
            Id = c.Id,
            CreatedAt = c.CreatedAt,
            SupplierName = c.Supplier != null ? c.Supplier.Name : "-",
            CollectAt = c.CollectAt,
            ProductDescription = c.Product != null ? c.Product.Description : "-",
            Status = c.Status,
            Volume = c.Volume,
            Weigth = c.Weigth,
            Filial = c.Filial != null ? c.Filial.Name : "-",
            ChangeStatus = new ChangeStatusCollectViewModel
            {
                Id = c.Id,
                Status = c.Status
            }
        }).ToList();

        return View(clivm);
    }

    public IActionResult CreateCollect()
    {
        return View();
    }

    public async Task<IActionResult> GetSuppliers()
    {
        List<Supplier> SuppliersList = await _collectService.GetRegisteredSuppliersAsync();
        return Json(SuppliersList);
    }

    public async Task<IActionResult> GetProducts()
    {
        List<Product> ProductsList = await _collectService.GetRegisteredProductsAsync();
        return Json(ProductsList);
    }

    public async Task<IActionResult> GetFilials()
    {
        List<Filial> FilialsList = await _collectService.GetRegisteredFilialsAsync();
        return Json(FilialsList);
    }

    [HttpPost]
    public async Task<IActionResult> FilterSuppliersList([FromBody] FilterRequestInputProduct request)
    {
        List<Supplier> SuppliersList = await _collectService.GetFilteredSuppliersAsync(request.Input);
        return Json(SuppliersList);
    }

    [HttpPost]
    public async Task<IActionResult> FilterProductsList([FromBody] FilterRequestInputProduct request)
    {
        List<Product> ProductsList = await _collectService.GetFilteredProductsAsync(request.Input);
        return Json(ProductsList);
    }

    [HttpPost]
    public async Task<IActionResult> FilterFilialsList([FromBody] FilterRequestInputProduct request)
    {
        List<Filial> FilialsList = await _collectService.GetFilteredFilialsAsync(request.Input);
        return Json(FilialsList);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCollect([Bind("SupplierId,Supplier,CollectAt,ProductId,Product,Volume,Weight,FilialId,Filial")] CreateCollectViewModel collectCreate)
    {
        if (!ModelState.IsValid)
        {
            return View(collectCreate);
        }

        if (collectCreate == null)
        {
            Console.WriteLine("a");
            return NotFound();
        }

        Supplier? supplier = await _collectService.FindSupplierAsync(collectCreate.SupplierId);
        Product? product = await _collectService.FindProductAsync(collectCreate.ProductId);
        Filial? filial = await _collectService.FindFilialAsync(collectCreate.FilialId);

        if (supplier == null || product == null || filial == null)
        {
            Console.WriteLine("b");
            Console.WriteLine(collectCreate.FilialId);
            return NotFound();
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
            Filial = filial,
        };

        _collectService.AddCollect(collect);
        await _collectService.SaveChangesCollectsAsync();

        return RedirectToAction(nameof(ListCollects));
    }

    public async Task<IActionResult> EditCollect(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Collect? collect = await _collectService.FindCollectAsync(id);

        if (collect == null)
        {
            return NotFound();
        }

        EditCollectViewModel ecvm = new EditCollectViewModel
        {
            Id = collect.Id,
            Supplier = collect.Supplier,
            CollectAt = collect.CollectAt,
            Volume = collect.Volume,
            Weight = collect.Weigth,
            Filial = collect.Filial,
        };

        return View(ecvm);
    }

    [HttpPost]
    public async Task<IActionResult> EditCollect([Bind("Id,Supplier,CollectAt,Volume,Weight,Filial")] EditCollectViewModel collectEdit)
    {
        if (!ModelState.IsValid)
        {
            return View(collectEdit);
        }

        Collect? collect = await _collectService.FindCollectAsync(collectEdit.Id);

        if (collect == null)
        {
            return NotFound();
        }

        collect.CollectAt = collectEdit.CollectAt;
        collect.Supplier = collectEdit.Supplier;
        collect.Volume = collectEdit.Volume;
        collect.Weigth = collectEdit.Weight;
        collect.Filial = collectEdit.Filial;

        await _collectService.SaveChangesCollectsAsync();

        return RedirectToAction(nameof(ListCollects));
    }

    [HttpPost]
    public async Task<IActionResult> ChangeCollectStatus([Bind("Id,Status")] ChangeStatusCollectViewModel changeStatus)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(ListCollects));
        }

        Collect? collect = await _collectService.FindCollectAsync(changeStatus.Id);

        if (collect == null)
        {
            return NotFound();
        }

        if (changeStatus.Status == null)
        {
            return RedirectToAction(nameof(ListCollects));
        }

        collect.Status = changeStatus.Status;

        _collectService.UpdateCollectStatus(collect);
        await _collectService.SaveChangesCollectsAsync();

        return RedirectToAction(nameof(ListCollects));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCollect(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Collect? collect = await _collectService.FindCollectAsync(id);

        if (collect == null)
        {
            return NotFound();
        }

        _collectService.DeleteCollect(collect);
        await _collectService.SaveChangesCollectsAsync();

        return RedirectToAction(nameof(ListCollects));
    }
}