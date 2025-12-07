using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Cosmetic_Shop.Services.Interfaces;
using Cosmetic_Shop.Repositories.Interfaces;
using CosmeticShop.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Cosmetic_Shop.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFavoriteService _favoriteService;
        private readonly UserManager<User> _userManager;

        public CategoryController(
            ICategoryService categoryService,
            ICategoryRepository categoryRepository,
            IFavoriteService favoriteService,
            UserManager<User> userManager)
        {
            _categoryService = categoryService;
            _categoryRepository = categoryRepository;
            _favoriteService = favoriteService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string category)
        {
            if (string.IsNullOrEmpty(category))
                return NotFound();

            var subcategories = _categoryService.GetSubcategories(category);
            if (subcategories == null || subcategories.Count == 0)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            int userId = user?.UserId ?? 0;
            ViewBag.UserId = userId;

            var products = await _categoryRepository.GetProductsBySubcategoriesAsync(subcategories);

            var favorites = await _favoriteService.GetFavoritesAsync(userId);
            var favoriteProductIds = favorites
                .Select(f => (int)f.GetType().GetProperty("productId")?.GetValue(f))
                .ToList();

            ViewBag.FavoriteProductIds = favoriteProductIds;
            ViewBag.Category = category;

            return View(products);
        }
    }
}
