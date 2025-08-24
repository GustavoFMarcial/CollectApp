using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CollectApp.Models;
using CollectApp.Services;
using CollectApp.ViewModels;

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

        _collectService.CreateCollect(collect);

        if (collect == null)
        {
            return NotFound();
        }

        await _collectService.SaveChangesCollectsAsync();

        return RedirectToAction(nameof(ListCollects));
    }
}
