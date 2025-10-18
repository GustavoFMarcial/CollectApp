using CollectApp.Services;
using CollectApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollectApp.Controllers;

[Authorize(Policy = "CanInsert")]
public class FilialController : Controller
{
    private readonly ILogger<FilialController> _logger;
    private readonly IFilialService _filialService;

    public FilialController(ILogger<FilialController> logger, IFilialService filialService)
    {
        _logger = logger;
        _filialService = filialService;
    }

    public async Task<IActionResult> ListFilials(FilialFilterViewModel filters, int pageNum = 1)
    {
        PagedResultViewModel<FilialListViewModel, FilialFilterViewModel> pagedResultFilialListViewModel = await _filialService.SetPagedResultFilialListViewModel(filters, pageNum);

        return View(pagedResultFilialListViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ListFilialsJson([FromBody] FilterRequestInput request)
    {
        FilialFilterViewModel ffvm = new FilialFilterViewModel();
        PagedResultViewModel<FilialListViewModel, FilialFilterViewModel> pagedResultFilialListViewModel = await _filialService.SetPagedResultFilialListViewModel(ffvm, request.PageNum, request.PageSize, request.Input);

        return Json(pagedResultFilialListViewModel);
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
}