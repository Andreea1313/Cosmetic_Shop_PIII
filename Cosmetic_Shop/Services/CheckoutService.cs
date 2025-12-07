using Cosmetic_Shop.Services.Interfaces;
using Cosmetic_Shop.Repositories.Interfaces;
using CosmeticShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cosmetic_Shop.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartService _cartService;
        private readonly ICheckoutRepository _checkoutRepo;

        public CheckoutService(ICartService cartService, ICheckoutRepository checkoutRepo)
        {
            _cartService = cartService;
            _checkoutRepo = checkoutRepo;
        }

        public async Task<Order> SubmitOrderAsync(Order model, int userId)
        {
            var cartItemsRaw = await _cartService.GetCartItemsAsync(userId);

            var cartItems = cartItemsRaw.Cast<dynamic>().ToList();

            if (!cartItems.Any())
                return null;

            decimal subtotal = 0;
            foreach (var ci in cartItems)
            {
                subtotal += (decimal)ci.price * (int)ci.quantity;
            }

            var shipping = subtotal > 50 ? 0 : 5;
            var total = subtotal + shipping;

            var newOrder = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Address = model.Address,
                City = model.City,
                ZIPCode = model.ZIPCode,
                PhoneNumber = model.PhoneNumber,
                PaymentTypeId = model.PaymentTypeId,
                TotalAmount = total
            };

            await _checkoutRepo.AddOrderAsync(newOrder);
            await _checkoutRepo.SaveChangesAsync();

            var orderProducts = cartItems.Select(ci => new OrderProduct
            {
                Order = newOrder,
                ProductId = (int)ci.productId,
                Quantity = (int)ci.quantity,
                PriceAtOrder = (decimal)ci.price
            }).ToList();

            await _checkoutRepo.AddOrderProductsAsync(orderProducts);
            await _checkoutRepo.SaveChangesAsync();

            await _cartService.ClearCartAsync(userId);

            return newOrder;
        }

    }

}
