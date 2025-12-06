using Microsoft.AspNetCore.Mvc;
using CollectApp.Models;
using CollectApp.Services;
using CollectApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CollectApp.Controllers;

public class CollectController : Controller
{
    private readonly ILogger<CollectController> _logger;
    private readonly ICollectService _collectService;
    private string CurrentUserId => HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

    public CollectController(ILogger<CollectController> logger, ICollectService collectService)
    {
        _logger = logger;
        _collectService = collectService;
    }

    public async Task<IActionResult> ListCollects(CollectFilterViewModel filters, int pageNum = 1)
    {
        PagedResultViewModel<CollectListViewModel, CollectFilterViewModel> clivm = await _collectService.SetPagedResultCollectListViewModel(filters, pageNum);

        return View(clivm);
    }

    [Authorize(Policy = "CanInsert")]
    public IActionResult CreateCollect()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Policy = "CanInsert")]
    public async Task<IActionResult> CreateCollect([Bind("SupplierId,Supplier,CollectAt,ProductId,Product,Volume,Weight,FilialId,Filial")] CreateCollectViewModel collectCreate)
    {
        if (!ModelState.IsValid)
        {
            return View(collectCreate);
        }

        await _collectService.CreateCollect(collectCreate, CurrentUserId);

        return RedirectToAction(nameof(ListCollects));
    }

    public async Task<IActionResult> EditCollect(int id)
    {
        bool isCollectOwner = await _collectService.MustBeCollectOwner();

        if (!isCollectOwner)
        {
            return RedirectToAction(nameof(ListCollects));
        }

        EditCollectViewModel? ecvm = await _collectService.SetEditCollectViewModel(id);

        if (ecvm == null)
        {
            return NotFound();
        }

        return View(ecvm);
    }

    [HttpPost]
    [Authorize(Policy = "CanEditAndDeleteCollect")]
    public async Task<IActionResult> EditCollect([Bind("Id,SupplierId,Supplier,CollectAt,ProductId,Product,Volume,Weight,FilialId,Filial")] EditCollectViewModel collectEdit)
    {
        bool isCollectOwner = await _collectService.MustBeCollectOwner();

        if (!isCollectOwner)
        {
            return RedirectToAction(nameof(ListCollects));
        }

        if (!ModelState.IsValid)
        {
            return View(collectEdit);
        }

        await _collectService.EditCollect(collectEdit);

        return RedirectToAction(nameof(ListCollects));
    }

    [HttpPost]
    [Authorize(Policy = "CanChangeCollectStatus")]
    public async Task<IActionResult> ChangeCollectStatus([Bind("Id")] ChangeCollectViewModel changeStatus)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(ListCollects));
        }

        await _collectService.UpdateCollectStatus(changeStatus);

        return RedirectToAction(nameof(ListCollects));
    }

    [HttpPost]
    public async Task<IActionResult> OpenCollect([Bind("Id,ToOpen")] ChangeCollectViewModel changeStatus)
    {
        bool isCollectOwner = await _collectService.MustBeCollectOwner();

        if (!ModelState.IsValid || !isCollectOwner)
        {
            return RedirectToAction(nameof(ListCollects));
        }

        await _collectService.UpdateCollectStatus(changeStatus);

        return RedirectToAction(nameof(ListCollects));
    }

    [HttpPost]
    [Authorize(Policy = "CanEditAndDeleteCollect")]
    public async Task<IActionResult> CancelCollect(int id)
    {
        bool isCollectOwner = await _collectService.MustBeCollectOwner();

        if (!isCollectOwner)
        {
            return RedirectToAction(nameof(ListCollects));
        }

        Collect? collect = await _collectService.FindCollectAsync(id);

        if (collect == null)
        {
            return NotFound();
        }

        await _collectService.CancelCollect(id);

        return RedirectToAction(nameof(ListCollects));
    }
}