using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CollectApp.Models;
using CollectApp.Services;
using CollectApp.ViewModels;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.CodeAnalysis.Differencing;

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
            Company = c.Company,
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
    public async Task<IActionResult> CreateCollect([Bind("Company,CollectAt,Volume,Weight,Filial")] CreateCollectViewModel collect)
    {
        if (!ModelState.IsValid)
        {
            return View(collect);
        }

        if (collect == null)
        {
            return NotFound();
        }

        Collect c = new Collect
        {
            Company = collect.Company,
            CollectAt = collect.CollectAt,
        };

        _collectService.AddCollect(c);
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
            Company = collect.Company,
            CollectAt = collect.CollectAt,
            Volume = collect.Volume,
            Weight = collect.Weigth,
            Filial = collect.Filial,
        };

        return View(ecvm);
    }

    [HttpPost]
    public async Task<IActionResult> EditCollect([Bind("Id,Company,CollectAt,Volume,Weight,Filial")] EditCollectViewModel collectEdit)
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
        collect.Company = collectEdit.Company;
        collect.Volume = collectEdit.Volume;
        collect.Weigth = collectEdit.Weight;
        collect.Filial = collectEdit.Filial;
        
        await _collectService.SaveChangesCollectsAsync();

        return RedirectToAction(nameof(ListCollects));
    }

    [HttpPost]
    public async Task<IActionResult> ChangeCollectStatus([Bind("Id,Status")] ChangeStatusCollectViewModel collect)
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

public class CollectCreateViewModel
{
}