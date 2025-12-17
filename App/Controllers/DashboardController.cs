using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollectApp.Controllers;

[Authorize(Roles="Admin,Gestor")]
public class DashboardController : Controller
{
    public IActionResult Dashboard()
    {
        return View();
    }
}