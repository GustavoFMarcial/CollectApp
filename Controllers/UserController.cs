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

        [HttpPost]
        public async Task<IActionResult> ChangeUserStatus(string id)
        {
            await _userService.ChangeUserStatus(id);

            return RedirectToAction(nameof(ListUsers));
        }

        public async Task<IActionResult> EditUser(string id)
        {
            EditUserViewModel esvm = await _userService.SetEditCollectViewModel(id);

            return View(esvm);
        }

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
}