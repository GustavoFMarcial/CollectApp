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

        public async Task<IActionResult> ListFilials(int pageNum = 1, int pageSize = 10)
        {
            PagedResultViewModel<FilialListViewModel> flvm = await _filialService.SetPagedResultFilialListViewModel(pageNum, pageSize);

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

            OperationResult result = await _filialService.CreateFilial(filialCreate);

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                ViewBag.ShowModal = true;
                return View(filialCreate);
            }

            return RedirectToAction(nameof(ListFilials));
        }

        public async Task<IActionResult> EditFilial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EditFilialViewModel epvm = await _filialService.SetEditFilialViewModel(id);

            return View(epvm);
        }

        [HttpPost]
        public async Task<IActionResult> EditFilial([Bind("Id,Name")] EditFilialViewModel filialEdit)
        {
            if (!ModelState.IsValid)
            {
                return View(filialEdit);
            }

            OperationResult result = await _filialService.EditFilial(filialEdit);

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                ViewBag.ShowModal = true;
                return View(filialEdit);
            }

            return RedirectToAction(nameof(ListFilials));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFilial(int? id)
        {
            OperationResult result = await _filialService.DeleteFilial(id);

            if (!result.Success)
            {
                TempData["Message"] = result.Message;
                TempData["ShowModal"] = true;
                return RedirectToAction(nameof(ListFilials));
            }

            return RedirectToAction(nameof(ListFilials));
        }

        public async Task<IActionResult> GetFilials()
        {
            PagedResultViewModel<FilialListViewModel> FilialsList = await _filialService.SetPagedResultFilialListViewModel();
            
            return Json(FilialsList);
        }
        
        [HttpPost]
        public async Task<IActionResult> FilterFilialsList([FromBody] FilterRequestInputProduct request)
        {
            List<Filial> FilialsList = await _filialService.GetFilteredFilialsAsync(request.Input);

            return Json(FilialsList);
        }
    }
}