using CosmeticShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cosmetic_Shop.Controllers
{
    [Authorize]
    public class Cart : Controller
    {
        private readonly UserManager<User> _userManager;

        public Cart(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            ViewBag.UserId = user.UserId;
            return View();
        }

    }
}
