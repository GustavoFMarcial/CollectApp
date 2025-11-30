using CollectApp.Services;
using CollectApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollectApp.Controllers;

[Authorize(Policy = "CanInsert")]
public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _productService;

    public ProductController(ILogger<ProductController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task<IActionResult> ListProducts(ProductFilterViewModel filters, int pageNum = 1)
    {
        PagedResultViewModel<ProductListViewModel, ProductFilterViewModel> pagedResultProductListViewModel = await _productService.SetPagedResultProductListViewModel(filters, pageNum);

        return View(pagedResultProductListViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ListProductsJson([FromBody] FilterRequestInput request)
    {
        ProductFilterViewModel pfvm = new ProductFilterViewModel();
        PagedResultViewModel<ProductListViewModel, ProductFilterViewModel> pagedResultProductListViewModel = await _productService.SetPagedResultProductListViewModel(pfvm, request.PageNum, request.PageSize, request.Input);

        return Json(pagedResultProductListViewModel);
    }

    public IActionResult CreateProduct()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([Bind("Name")] CreateProductViewModel productCreate)
    {
        if (!ModelState.IsValid)
        {
            return View(productCreate);
        }

        OperationResult result = await _productService.CreateProduct(productCreate);

        if (!result.Success)
        {
            ViewBag.Message = result.Message;
            ViewBag.ShowModal = true;
            return View(productCreate);
        }

        return RedirectToAction(nameof(ListProducts));
    }

    public async Task<IActionResult> EditProduct(int id)
    {
        EditProductViewModel? epvm = await _productService.SetEditProductViewModel(id);

        if (epvm == null)
        {
            return NotFound();
        }

        return View(epvm);
    }

    [HttpPost]
    public async Task<IActionResult> EditProduct([Bind("Id,Name")] EditProductViewModel productEdit)
    {
        if (!ModelState.IsValid)
        {
            return View(productEdit);
        }

        OperationResult? result = await _productService.EditProduct(productEdit);

        if (result == null)
        {
            return NotFound();
        }

        if (!result.Success)
        {
            ViewBag.Message = result.Message;
            ViewBag.ShowModal = true;
            return View(productEdit);
        }

        return RedirectToAction(nameof(ListProducts));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        OperationResult? result = await _productService.DeleteProduct(id);

        if (result == null)
        {
            return NotFound();
        }

        if (!result.Success)
        {
            TempData["Message"] = result.Message;
            TempData["ShowModal"] = true;
            return RedirectToAction(nameof(ListProducts));
        }

        return RedirectToAction(nameof(ListProducts));
    }
}