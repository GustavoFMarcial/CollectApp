using CollectApp.Models;
using CollectApp.Services;
using CollectApp.ViewModels;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;

namespace CollectApp.Controllers
{
    public class SupplierController : Controller
    {
        public readonly ILogger<SupplierController> _logger;
        public readonly ISupplierService _supplierService;

        public SupplierController(ILogger<SupplierController> logger, ISupplierService supplierService)
        {
            _logger = logger;
            _supplierService = supplierService;
        }

        public async Task<IActionResult> ListSuppliers()
        {
            List<Supplier> suppliers = await _supplierService.GetAllSuppliersListAsycn();

            List<SupplierListViewModel> slvm = suppliers.Select(s => new SupplierListViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Address = s.Address,
                CNPJ = s.CNPJ,
            }).ToList();

            return View(slvm);
        }

        public IActionResult CreateSupplier()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupplier([Bind("Name,CNPJ,Address")] CreateSupplierViewModel supplierCreate)
        {
            if (!ModelState.IsValid)
            {
                return View(supplierCreate);
            }

            if (supplierCreate == null)
            {
                return NotFound();
            }

            Supplier supplier = new Supplier
            {
                Name = supplierCreate.Name,
                CNPJ = supplierCreate.CNPJ,
                Address = supplierCreate.Address,
            };

            _supplierService.AddSupplier(supplier);
            await _supplierService.SaveChangesSuppliersAsync();

            Console.WriteLine(supplier.Id);

            return RedirectToAction(nameof(ListSuppliers));
        }

        public async Task<IActionResult> EditSupplier(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Supplier? supplier = await _supplierService.FindSupplierAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            EditSupplierViewModel esvm = new EditSupplierViewModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                CNPJ = supplier.CNPJ,
                Address = supplier.Address,
            };

            return View(esvm);
        }

        [HttpPost]
        public async Task<IActionResult> EditSupplier([Bind("Id,Name,CNPJ,Address")] EditSupplierViewModel supplierEdit)
        {
            if (!ModelState.IsValid)
            {
                return View(supplierEdit);
            }

            Supplier? supplier = await _supplierService.FindSupplierAsync(supplierEdit.Id);

            if (supplier == null)
            {
                return NotFound();
            }

            supplier.Name = supplierEdit.Name;
            supplier.CNPJ = supplierEdit.CNPJ;
            supplier.Address = supplierEdit.Address;

            await _supplierService.SaveChangesSuppliersAsync();

            return RedirectToAction(nameof(ListSuppliers));
        }
    }
}