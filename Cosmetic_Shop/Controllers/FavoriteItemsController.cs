using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CosmeticShop.Models;
using Cosmetic_Shop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Cosmetic_Shop.Controllers
{
    [Authorize]
    public class FavoriteItemsController : Controller
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteItemsController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToFavorites([FromBody] Favorite model)
        {
            if (model == null || model.UserId <= 0 || model.ProductId <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid or missing data." });
            }

            var added = await _favoriteService.AddToFavoritesAsync(model);
            if (!added)
            {
                return Json(new { success = false, message = "Already in favorites." });
            }

            return Json(new { success = true, message = "Added to favorites." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromFavorites([FromBody] Favorite model)
        {
            var removed = await _favoriteService.RemoveFromFavoritesAsync(model);
            if (!removed)
                return NotFound();

            return Json(new { success = true, message = "Removed from favorites." });
        }

        [HttpGet]
        public async Task<IActionResult> GetFavorites(int userId)
        {
            var favorites = await _favoriteService.GetFavoritesAsync(userId);
            return Json(favorites);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ToggleFavorite([FromBody] Favorite dto)
        {
            if (dto == null || dto.UserId <= 0 || dto.ProductId <= 0)
                return Json(new { success = false, message = "Invalid data" });

            var result = await _favoriteService.ToggleFavoriteAsync(dto);
            return Json(new { success = true, pressed = result });
        }

    }
}