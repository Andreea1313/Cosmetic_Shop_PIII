using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Cosmetic_Shop.Services.Interfaces;
using CosmeticShop.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cosmetic_Shop.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(UserManager<User> userManager, IFavoriteService favoriteService)
        {
            _userManager = userManager;
            _favoriteService = favoriteService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            ViewBag.UserId = user.UserId;

            var favorites = await _favoriteService.GetFavoritesAsync(user.UserId);

            return View(favorites); 
        }
    }
}
