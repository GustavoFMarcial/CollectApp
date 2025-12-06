using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using CollectApp.Models;
using System.Diagnostics;

namespace CollectApp.Controllers;

public class SharedController : Controller
{
    private readonly ILogger<SharedController> _logger;

    public SharedController(ILogger<SharedController> logger)
    {
        _logger = logger;
    }

    [Route("Shared/Error")]
    public IActionResult Error()
    {
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionFeature?.Error;
        string requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        _logger.LogError(exception, "Ocorreu um erro.");

        ErrorViewModel evm = new ErrorViewModel
        {
            RequestId = requestId,
        };

        return View(evm);
    }
}
