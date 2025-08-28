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
        public async Task<IActionResult> CreateProduct([Bind("SKU,Description")] CreateProductViewModel productCreate)
        {
            if (!ModelState.IsValid)
            {
                return View(productCreate);
            }

            Product product = new Product
            {
                Description = productCreate.Description,
            };

            _productService.AddProduct(product);
            await _productService.SaveChangesProductsAsync();

            return RedirectToAction(nameof(ListProducts));
        }
    }
}