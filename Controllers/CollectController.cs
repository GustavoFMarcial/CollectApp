using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CollectApp.Models;
using CollectApp.Services;
using CollectApp.ViewModels;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace CollectApp.Controllers;

public class CollectController : Controller
{
    private readonly ILogger<CollectController> _looger;
    private readonly ICollectService _collectService;

    public CollectController(ILogger<CollectController> logger, ICollectService collectService)
    {
        _looger = logger;
        _collectService = collectService;
    }

    public async Task<IActionResult> ListCollects()
    {
        return View(await _collectService.GetAllCollectsListAsycn());
    }

    public IActionResult CreateCollect()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCollect([Bind("Company,CollectAt")] CollectCreateViewModel collect)
    {
        if (!ModelState.IsValid)
        {
            return View(collect);
        }

        if (collect == null)
        {
            return NotFound();
        }

        _collectService.CreateCollect(collect);

        await _collectService.SaveChangesCollectsAsync();

        return RedirectToAction(nameof(ListCollects));
    }

    [HttpPost]
    public async Task<IActionResult> ChangeCollectStatus([Bind("Id,Status")] CollectChangeStatusViewModel collect)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(ListCollects));
        }

        Collect? _collect = await _collectService.FindCollectAsync(collect.Id);

        if (_collect == null)
        {
            return NotFound();
        }

        if (collect.Status == null)
        {
           return RedirectToAction(nameof(ListCollects)); 
        }

        _collectService.UpdateCollectStatus(_collect, collect.Status);
        await _collectService.SaveChangesCollectsAsync();

        return RedirectToAction(nameof(ListCollects));
    }
}
