using CollectApp.Models;
using CollectApp.Services;
using CollectApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CollectApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> ListProducts(int pageNum = 1)
        {
            PagedResultViewModel<ProductListViewModel> pagedResultProductListViewModel = await _productService.SetPagedResultProductListViewModel(pageNum);

            return View(pagedResultProductListViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ListProductsJson([FromBody] FilterRequestInput request)
        {
            PagedResultViewModel<ProductListViewModel> pagedResultProductListViewModel = await _productService.SetPagedResultProductListViewModel(request.PageNum, request.PageSize, request.Input);
            
            return Json(pagedResultProductListViewModel);
        }

        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([Bind("Description")] CreateProductViewModel productCreate)
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

        public async Task<IActionResult> EditProduct(int? id)
        {
            EditProductViewModel epvm = await _productService.SetEditProductViewModel(id);

            return View(epvm);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct([Bind("Id,Description")] EditProductViewModel productEdit)
        {
            if (!ModelState.IsValid)
            {
                return View(productEdit);
            }

            OperationResult result = await _productService.EditProduct(productEdit);

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                ViewBag.ShowModal = true;
                return View(productEdit);
            }

            return RedirectToAction(nameof(ListProducts));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            OperationResult result = await _productService.DeleteProduct(id);

            if (!result.Success)
            {
                TempData["Message"] = result.Message;
                TempData["ShowModal"] = true;
                return RedirectToAction(nameof(ListProducts));
            }

            return RedirectToAction(nameof(ListProducts));
        }
    }
}