using CollectApp.Models;
using CollectApp.Services;
using CollectApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;

namespace CollectApp.Controllers
{
    public class FilialController : Controller
    {
        private readonly ILogger<FilialController> _logger;
        private readonly IFilialService _filialService;

        public FilialController(ILogger<FilialController> logger, IFilialService filialService)
        {
            _logger = logger;
            _filialService = filialService;
        }

        public async Task<IActionResult> ListFilials()
        {
            List<Filial> filials = await _filialService.GetAllFilialsListAsycn();

            List<FilialListViewModel> flvm = filials.Select(f => new FilialListViewModel
            {
                Id = f.Id,
                Name = f.Name
            }).ToList();

            return View(flvm);
        }

        public IActionResult CreateFilial()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFilial([Bind("Name")] CreateFilialViewModel filialCreate)
        {
            if (!ModelState.IsValid)
            {
                return View(filialCreate);
            }

            Filial filial = new Filial
            {
                Name = filialCreate.Name,
            };

            OperationResult result = await _filialService.AddFilial(filial);

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                ViewBag.ShowModal = true;
                return View(filialCreate);
            }

            await _filialService.SaveChangesFilialsAsync();

            return RedirectToAction(nameof(ListFilials));
        }

        public async Task<IActionResult> EditFilial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Filial? filial = await _filialService.FindFilialAsync(id);

            if (filial == null)
            {
                return NotFound();
            }

            EditFilialViewModel epvm = new EditFilialViewModel
            {
                Id = filial.Id,
                Name = filial.Name,
            };

            return View(epvm);
        }

        [HttpPost]
        public async Task<IActionResult> EditFilial([Bind("Id,Name")] EditFilialViewModel filialEdit)
        {
            if (!ModelState.IsValid)
            {
                return View(filialEdit);
            }

            Filial? filial = await _filialService.FindFilialAsync(filialEdit.Id);;

            if (filial == null)
            {
                return NotFound();
            }

            OperationResult result = await _filialService.EditFilial(filialEdit);

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                ViewBag.ShowModal = true;
                return View(filialEdit);
            }

            filial.Name = filialEdit.Name;
            await _filialService.SaveChangesFilialsAsync();

            return RedirectToAction(nameof(ListFilials));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFilial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Filial? filial = await _filialService.FindFilialAsync(id);

            if (filial == null)
            {
                return NotFound();
            }

            _filialService.DeleteFilial(filial);
            await _filialService.SaveChangesFilialsAsync();

            return RedirectToAction(nameof(ListFilials));
        }
    }
}