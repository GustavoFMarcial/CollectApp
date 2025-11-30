using CollectApp.Services;
using CollectApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollectApp.Controllers;

[Authorize(Policy = "CanInsert")]
public class SupplierController : Controller
{
    public readonly ILogger<SupplierController> _logger;
    public readonly ISupplierService _supplierService;

    public SupplierController(ILogger<SupplierController> logger, ISupplierService supplierService)
    {
        _logger = logger;
        _supplierService = supplierService;
    }

    public async Task<IActionResult> ListSuppliers(SupplierFilterViewModel filters, int pageNum = 1)
    {
        PagedResultViewModel<SupplierListViewModel, SupplierFilterViewModel> pagedResultSupplierListViewModel = await _supplierService.SetPagedResultSupplierListViewModel(filters, pageNum);

        return View(pagedResultSupplierListViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ListSuppliersJson([FromBody] FilterRequestInput request)
    {
        SupplierFilterViewModel sfvm = new SupplierFilterViewModel();
        PagedResultViewModel<SupplierListViewModel, SupplierFilterViewModel> pagedResultSupplierListViewModel = await _supplierService.SetPagedResultSupplierListViewModel(sfvm, request.PageNum, request.PageSize, request.Input);

        return Json(pagedResultSupplierListViewModel);
    }

    public IActionResult CreateSupplier()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateSupplier([Bind("Name,CNPJ,Street,Neighborhood,Number,City,State,ZipCode")] CreateSupplierViewModel supplierCreate)
    {
        if (!ModelState.IsValid)
        {
            return View(supplierCreate);
        }

        OperationResult? result = await _supplierService.CreateSupplier(supplierCreate);

        if (result == null)
        {
            return NotFound();
        }

        if (!result.Success)
        {
            ViewBag.Message = result.Message;
            ViewBag.ShowModal = true;
            return View(supplierCreate);
        }

        return RedirectToAction(nameof(ListSuppliers));
    }

    public async Task<IActionResult> EditSupplier(int id)
    {
        EditSupplierViewModel? esvm = await _supplierService.SetEditSupplierViewModel(id);

        if (esvm == null)
        {
            return NotFound();
        }

        return View(esvm);
    }

    [HttpPost]
    public async Task<IActionResult> EditSupplier([Bind("Id,Name,CNPJ,Street,Neighborhood,Number,City,State,ZipCode")] EditSupplierViewModel supplierEdit)
    {
        if (!ModelState.IsValid)
        {
            return View(supplierEdit);
        }

        OperationResult? result = await _supplierService.EditSupplier(supplierEdit);

        if (result == null)
        {
            return NotFound();
        }

        if (!result.Success)
        {
            ViewBag.Message = result.Message;
            ViewBag.ShowModal = true;
            return View(supplierEdit);
        }

        return RedirectToAction(nameof(ListSuppliers));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        OperationResult? result = await _supplierService.DeleteSupplier(id);

        if (result == null)
        {
            return NotFound();
        }

        if (!result.Success)
        {
            TempData["Message"] = result.Message;
            TempData["ShowModal"] = true;
            return RedirectToAction(nameof(ListSuppliers));
        }

        return RedirectToAction(nameof(ListSuppliers));
    }
}