using CollectApp.Models;
using CollectApp.Services;
using CollectApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;

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

        public async Task<IActionResult> ListProducts()
        {
            List<Product> products = await _productService.GetAllProductsListAsycn();

            List<ProductListViewModel> plvm = products.Select(p => new ProductListViewModel
            {
                Id = p.Id,
                Description = p.Description
            }).ToList();

            return View(plvm);
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

            Product product = new Product
            {
                Description = productCreate.Description,
            };

            OperationResult result = await _productService.AddProduct(product);

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                ViewBag.ShowModal = true;
                return View(productCreate);
            }

            await _productService.SaveChangesProductsAsync();

            return RedirectToAction(nameof(ListProducts));
        }

        public async Task<IActionResult> EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product? product = await _productService.FindProductAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            EditProductViewModel epvm = new EditProductViewModel
            {
                Id = product.Id,
                Description = product.Description,
            };

            return View(epvm);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct([Bind("Id,Description")] EditProductViewModel productEdit)
        {
            if (!ModelState.IsValid)
            {
                return View(productEdit);
            }

            Product? product = await _productService.FindProductAsync(productEdit.Id); ;

            if (product == null)
            {
                return NotFound();
            }

            OperationResult result = await _productService.EditProduct(productEdit);

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                ViewBag.ShowModal = true;
                return View(productEdit);
            }

            product.Description = productEdit.Description;
            await _productService.SaveChangesProductsAsync();

            return RedirectToAction(nameof(ListProducts));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product? product = await _productService.FindProductAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _productService.DeleteProduct(product);
            await _productService.SaveChangesProductsAsync();

            return RedirectToAction(nameof(ListProducts));
        }

        public async Task<IActionResult> GetProducts()
        {
            List<Product> ProductsList = await _productService.GetAllProductsListAsycn();
            return Json(ProductsList);
        }
        
        [HttpPost]
        public async Task<IActionResult> FilterProductsList([FromBody] FilterRequestInputProduct request)
        {
            List<Product> ProductsList = await _productService.GetFilteredProductsAsync(request.Input);
            return Json(ProductsList);
        }
    }
}