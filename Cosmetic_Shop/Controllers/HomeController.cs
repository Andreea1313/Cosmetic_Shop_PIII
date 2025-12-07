using Cosmetic_Shop.Services.Interfaces;
using CosmeticShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cosmetic_Shop.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly CosmeticShopContext _context;
        private readonly UserManager<User> _userManager; 

       
        public HomeController(IHomeService homeService, CosmeticShopContext context, UserManager<User> userManager)
        {
            _homeService = homeService;
            _context = context;
            _userManager = userManager; 
        }

        public async Task<IActionResult> Index()
        {
            var (products, favoriteIds, userId) = await _homeService.GetHomeDataAsync(User);
            ViewBag.UserId = userId;
            ViewBag.FavoriteProductIds = favoriteIds;
            return View(products);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ViewAccounts()
        {
            var users = await _homeService.GetAllUserViewModelsAsync();
            return View("ViewAccounts", users);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ManageProducts()
        {
            ViewBag.Categories = GetMainCategories();
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = GetMainCategories();
                var all = await _context.Products.ToListAsync();
                return View("ManageProducts", all);
            }

            if (product.ProductId == 0)
                _context.Products.Add(product);
            else
                _context.Products.Update(product);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageProducts));
        }

        [Authorize]
        public async Task<IActionResult> ViewOrders()
        {
            var identityUser = await _userManager.GetUserAsync(User);

            if (identityUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int numericUserId = identityUser.UserId;

          
            var orders = await _context.Orders
                .Where(o => o.UserId == numericUserId)
                .Include(o => o.PaymentType)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

  
            return View("~/Views/Home/ViewOrders.cshtml", orders);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var prod = await _context.Products.FindAsync(id);
            if (prod != null)
            {
                _context.Products.Remove(prod);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageProducts));
        }

        private List<string> GetMainCategories() => new()
        {
            "hair",
            "skincare",
            "makeup",
            "bodycare",
            "nails"
        };
    }
}
