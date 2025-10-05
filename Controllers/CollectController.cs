using Microsoft.AspNetCore.Mvc;
using CollectApp.Models;
using CollectApp.Services;
using CollectApp.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CollectApp.Controllers;

public class CollectController : Controller
{
    private readonly ILogger<CollectController> _logger;
    private readonly ICollectService _collectService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public CollectController(ILogger<CollectController> logger, ICollectService collectService, ICurrentUserService currentUserService, IAuthorizationService authorizationService)
    {
        _logger = logger;
        _collectService = collectService;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<IActionResult> ListCollects(int pageNum = 1)
    {
        PagedResultViewModel<CollectListViewModel> clivm = await _collectService.SetPagedResultCollectListViewModel(pageNum);

        return View(clivm);
    }

    public IActionResult CreateCollect()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Policy = "CanCreateEditOrDelete")]
    public async Task<IActionResult> CreateCollect([Bind("SupplierId,Supplier,CollectAt,ProductId,Product,Volume,Weight,FilialId,Filial")] CreateCollectViewModel collectCreate)
    {
        if (!ModelState.IsValid)
        {
            return View(collectCreate);
        }

        if (collectCreate == null)
        {
            return NotFound();
        }

        await _collectService.CreateCollect(collectCreate, _currentUserService.UserId);

        return RedirectToAction(nameof(ListCollects));
    }

    public async Task<IActionResult> EditCollect(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        EditCollectViewModel ecvm = await _collectService.SetEditCollectViewModel(id);

        return View(ecvm);
    }

    [HttpPost]
    [Authorize(Policy = "CanCreateEditOrDelete")]
    public async Task<IActionResult> EditCollect([Bind("Id,SupplierId,Supplier,CollectAt,ProductId,Product,Volume,Weight,FilialId,Filial")] EditCollectViewModel collectEdit)
    {
        bool isCollectOwner = (await _authorizationService.AuthorizeAsync(_currentUserService.User, "MustBeCollectOwner")).Succeeded;

        if (!isCollectOwner)
        {
            return Forbid();
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
    public async Task<IActionResult> ChangeCollectStatus([Bind("Id,ToOpen")] ChangeCollectViewModel changeStatus)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(ListCollects));
        }

        await _collectService.UpdateCollectStatus(changeStatus);

        return RedirectToAction(nameof(ListCollects));
    }

    [HttpPost]
    [Authorize(Policy = "CanCreateEditOrDelete")]
    public async Task<IActionResult> DeleteCollect(int? id)
    {
        bool isCollectOwner = (await _authorizationService.AuthorizeAsync(_currentUserService.User, "MustBeCollectOwner")).Succeeded;

        if (!isCollectOwner)
        {
            return Forbid();
        }

        if (id == null)
        {
            return NotFound();
        }

        Collect? collect = await _collectService.FindCollectAsync(id);

        if (collect == null)
        {
            return NotFound();
        }

        await _collectService.DeleteCollect(id);

        return RedirectToAction(nameof(ListCollects));
    }
}