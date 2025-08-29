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
        List<Collect> collects = await _collectService.GetAllCollectsListAsycn();

        List<CollectListItemViewModel> clivm = collects.Select(c => new CollectListItemViewModel
        {
            Id = c.Id,
            CreatedAt = c.CreatedAt,
            Supplier = c.Supplier,
            CollectAt = c.CollectAt,
            Status = c.Status,
            Volume = c.Volume,
            Weigth = c.Weigth,
            Filial = c.Filial,
            ChangeStatus = new ChangeStatusCollectViewModel
            {
                Id = c.Id,
                Status = c.Status
            }
        }).ToList();

        return View(clivm);
    }

    public IActionResult CreateCollect()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCollect([Bind("Supplier,CollectAt,Volume,Weight,Filial")] CreateCollectViewModel collectCreate)
    {
        if (!ModelState.IsValid)
        {
            return View(collectCreate);
        }

        if (collectCreate == null)
        {
            return NotFound();
        }

        Collect collect = new Collect
        {
            Supplier = collectCreate.Supplier,
            CollectAt = collectCreate.CollectAt,
            Volume = collectCreate.Volume,
            Weigth = collectCreate.Weight,
            Filial = collectCreate.Filial,
        };

        _collectService.AddCollect(collect);
        await _collectService.SaveChangesCollectsAsync();

        return RedirectToAction(nameof(ListCollects));
    }

    public async Task<IActionResult> EditCollect(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Collect? collect = await _collectService.FindCollectAsync(id);

        if (collect == null)
        {
            return NotFound();
        }

        EditCollectViewModel ecvm = new EditCollectViewModel
        {
            Id = collect.Id,
            Supplier = collect.Supplier,
            CollectAt = collect.CollectAt,
            Volume = collect.Volume,
            Weight = collect.Weigth,
            Filial = collect.Filial,
        };

        return View(ecvm);
    }

    [HttpPost]
    public async Task<IActionResult> EditCollect([Bind("Id,Supplier,CollectAt,Volume,Weight,Filial")] EditCollectViewModel collectEdit)
    {
        if (!ModelState.IsValid)
        {
            return View(collectEdit);
        }

        Collect? collect = await _collectService.FindCollectAsync(collectEdit.Id);

        if (collect == null)
        {
            return NotFound();
        }

        collect.CollectAt = collectEdit.CollectAt;
        collect.Supplier = collectEdit.Supplier;
        collect.Volume = collectEdit.Volume;
        collect.Weigth = collectEdit.Weight;
        collect.Filial = collectEdit.Filial;

        await _collectService.SaveChangesCollectsAsync();

        return RedirectToAction(nameof(ListCollects));
    }

    [HttpPost]
    public async Task<IActionResult> ChangeCollectStatus([Bind("Id,Status")] ChangeStatusCollectViewModel changeStatus)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(ListCollects));
        }

        Collect? collect = await _collectService.FindCollectAsync(changeStatus.Id);

        if (collect == null)
        {
            return NotFound();
        }

        if (changeStatus.Status == null)
        {
            return RedirectToAction(nameof(ListCollects));
        }

        collect.Status = changeStatus.Status;

        _collectService.UpdateCollectStatus(collect);
        await _collectService.SaveChangesCollectsAsync();

        return RedirectToAction(nameof(ListCollects));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCollect(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Collect? collect = await _collectService.FindCollectAsync(id);

        if (collect == null)
        {
            return NotFound();
        }

        _collectService.DeleteCollect(collect);
        await _collectService.SaveChangesCollectsAsync();

        return RedirectToAction(nameof(ListCollects));
    }
}