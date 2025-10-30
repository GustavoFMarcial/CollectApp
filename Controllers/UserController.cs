using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CollectApp.Services;
using CollectApp.ViewModels;

namespace CollectApp.Controllers;


public class UserController : Controller
{
    public readonly IUserService _userService;
    public readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [Authorize(Policy = "CanCreateAndEditUsers")]
    public async Task<IActionResult> ListUsers(UserFilterViewModel filters, int pageNum = 1)
    {
        PagedResultViewModel<UserListViewModel, UserFilterViewModel> clivm = await _userService.SetPagedResultUserListViewModel(filters, pageNum);

        return View(clivm);
    }

    [Authorize(Policy = "CanCreateAndEditUsers")]
    [HttpPost]
    public async Task<IActionResult> ChangeUserStatus(string id)
    {
        await _userService.ChangeUserStatus(id);

        return RedirectToAction(nameof(ListUsers));
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login([Bind("Username,Password")] LoginViewModel credentials)
    {
        string returnUrl = Url.Content("~/");

        if (!ModelState.IsValid)
        {
            return View(credentials);
        }

        Microsoft.AspNetCore.Identity.SignInResult result = await _userService.LogIn(credentials);

        if (result.Succeeded)
        {
            _logger.LogInformation("User logged in.");
            return LocalRedirect(returnUrl);
        }
        if (result.IsLockedOut)
        {
            _logger.LogWarning("User account locked out.");
            return RedirectToPage("./Lockout");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }
    }

    [Authorize(Policy = "CanCreateAndEditUsers")]
    public IActionResult CreateUser()
    {
        return View();
    }

    [Authorize(Policy = "CanCreateAndEditUsers")]
    [HttpPost]
    public async Task<IActionResult> CreateUser([Bind("FullName,Username,Password,ConfirmPassword,Role")] CreateUserViewModel createUser)
    {
        if (!ModelState.IsValid)
        {
            return View(createUser);
        }

        OperationResult result = await _userService.CreateUser(createUser);

        if (!result.Success)
        {
            ViewBag.Message = result.Message;
            ViewBag.ShowModal = true;
            return View(createUser);
        }

        return RedirectToAction(nameof(ListUsers));
    }

    [Authorize(Policy = "CanCreateAndEditUsers")]
    public async Task<IActionResult> EditUser(string id)
    {
        EditUserViewModel esvm = await _userService.SetEditCollectViewModel(id);

        return View(esvm);
    }

    [Authorize(Policy = "CanCreateAndEditUsers")]
    [HttpPost]
    public async Task<IActionResult> EditUser([Bind("Id,FullName,Role")] EditUserViewModel editUser)
    {
        if (!ModelState.IsValid)
        {
            return View(editUser);
        }

        OperationResult result = await _userService.EditUser(editUser);

        if (!result.Success)
        {
            ViewBag.Message = result.Message;
            ViewBag.ShowModal = true;
            return View(editUser);
        }

        return RedirectToAction(nameof(ListUsers));
    }
}