using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CollectApp.Services;
using CollectApp.ViewModels;

namespace CollectApp.Controllers
{
    [Authorize(Policy = "CanCreateAndEditUsers")]
    public class UserController : Controller
    {
        public readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> ListUsers(int pageNum = 1)
        {
            PagedResultViewModel<UserListViewModel> clivm = await _userService.SetPagedResultUserListViewModel(pageNum);

            return View(clivm);
        }
    }
}