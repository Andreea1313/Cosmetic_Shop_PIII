using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CosmeticShop.Models;
using CartModel = CosmeticShop.Models.Cart;
using Cosmetic_Shop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace Cosmetic_Shop.Controllers
{
    [Authorize]
    public class CartItemsController : Controller
    {
        private readonly ICartService _cartService;

        public CartItemsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int userId, int productId, int quantity = 1)
        {
            var success = await _cartService.AddToCartAsync(userId, productId, quantity);
            if (!success)
                return BadRequest(new { success = false, message = "Invalid data." });

            return Json(new { success = true, message = "Item added to cart." });
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems(int userId)
        {
            var result = await _cartService.GetCartItemsAsync(userId);
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity([FromBody] CartItem model)
        {
            var success = await _cartService.UpdateQuantityAsync(model.CartItemId, model.Quantity);
            if (!success) return NotFound();

            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove([FromBody] CartItem model)
        {
            var success = await _cartService.RemoveItemAsync(model.CartItemId);
            if (!success) return NotFound();

            return Json(new { success = true });
        }
    }

}
