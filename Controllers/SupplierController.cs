using System.Linq.Expressions;
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
                CNPJ = s.CNPJ,
                Address = $"Rua {s.Street}, Bairro {s.Neighborhood}, nº {s.Number}, {s.City}/{s.State} - CEP {s.ZipCode}",
            }).ToList();

            return View(slvm);
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

            if (supplierCreate == null)
            {
                return NotFound();
            }

            Supplier supplier = new Supplier
            {
                Name = supplierCreate.Name,
                CNPJ = supplierCreate.CNPJ,
                Street = supplierCreate.Street,
                Neighborhood = supplierCreate.Neighborhood,
                Number = supplierCreate.Number,
                City = supplierCreate.City,
                State = supplierCreate.State,
                ZipCode = supplierCreate.ZipCode,
            };

            OperationResult result = await _supplierService.AddSupplier(supplier);

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                ViewBag.ShowModal = true;
                return View(supplierCreate);
            }

            await _supplierService.SaveChangesSuppliersAsync();

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
                Street = supplier.Street,
                Neighborhood = supplier.Neighborhood,
                Number = supplier.Number,
                City = supplier.City,
                State = supplier.State,
                ZipCode = supplier.ZipCode,
            };

            return View(esvm);
        }

        [HttpPost]
        public async Task<IActionResult> EditSupplier([Bind("Id,Name,CNPJ,Street,Neighborhood,Number,City,State,ZipCode")] EditSupplierViewModel supplierEdit)
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

            OperationResult result = await _supplierService.EditSupplier(supplier);

            if (!result.Success && supplierEdit.Id != supplier.Id)
            {
                ViewBag.Message = result.Message;
                ViewBag.ShowModal = true;
                return View(supplierEdit);
            }

            supplier.Name = supplierEdit.Name;
            supplier.CNPJ = supplierEdit.CNPJ;
            supplier.Street = supplierEdit.Street;
            supplier.Neighborhood = supplierEdit.Neighborhood;
            supplier.Number = supplierEdit.Number;
            supplier.City = supplierEdit.City;
            supplier.State = supplierEdit.State;
            supplier.ZipCode = supplierEdit.ZipCode;

            await _supplierService.SaveChangesSuppliersAsync();

            return RedirectToAction(nameof(ListSuppliers));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSupplier(int? id)
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

            _supplierService.DeleteSupplier(supplier);
            await _supplierService.SaveChangesSuppliersAsync();

            return RedirectToAction(nameof(ListSuppliers));
        }
    }
}