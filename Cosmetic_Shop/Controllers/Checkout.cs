using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Cosmetic_Shop.Services.Interfaces;
using CosmeticShop.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Cosmetic_Shop.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICheckoutService _checkoutService;
        private readonly UserManager<User> _userManager;

        public CheckoutController(
            ICartService cartService,
            ICheckoutService checkoutService,
            UserManager<User> userManager)
        {
            _cartService = cartService;
            _checkoutService = checkoutService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var cartItems = await _cartService.GetCartItemsAsync(user.UserId);
            ViewBag.UserId = user.UserId;
            ViewBag.CartItems = cartItems;

            return View();
        }

        public IActionResult OrderConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitOrder(Order model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var order = await _checkoutService.SubmitOrderAsync(model, user.UserId);
            if (order == null)
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("OrderConfirmation");
        }
    }
}
