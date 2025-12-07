using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cosmetic_Shop.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Cosmetic_Shop.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetCurrentUserAsync(User);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(IFormFile profileImage)
        {
            await _userService.UpdateProfilePictureAsync(User, profileImage);
            return RedirectToAction("Index");
        }
    }
}
