using Cosmetic_Shop.Repositories.Interfaces;
using Cosmetic_Shop.Services.Interfaces;
using CosmeticShop.Models;

namespace Cosmetic_Shop.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly ICartItemRepository _itemRepo;

        public CartService(ICartRepository cartRepo, ICartItemRepository itemRepo)
        {
            _cartRepo = cartRepo;
            _itemRepo = itemRepo;
        }
        public async Task<bool> ClearCartAsync(int userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null) return false;

            await _itemRepo.RemoveAllItemsFromCartAsync(cart.CartId);
            await _cartRepo.SaveChangesAsync();
            return true;
        }


        public async Task<bool> AddToCartAsync(int userId, int productId, int quantity)
        {
            if (userId <= 0 || productId <= 0 || quantity <= 0)
                return false;

            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _cartRepo.AddCartAsync(cart);
                await _cartRepo.SaveChangesAsync();
            }

            var existingItem = await _itemRepo.GetCartItemAsync(cart.CartId, productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _itemRepo.AddCartItemAsync(newItem);
            }

            await _cartRepo.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<object>> GetCartItemsAsync(int userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null || cart.CartItems == null)
                return new List<object>();

            return cart.CartItems.Select(ci => new
            {
                productId = ci.ProductId,
                cartItemId = ci.CartItemId,
                name = ci.Product.Name,
                description = ci.Product.Description,
                image = ci.Product.ImageUrl,
                price = ci.Product.Price,
                quantity = ci.Quantity
            });
        }

        public async Task<bool> UpdateQuantityAsync(int cartItemId, int quantity)
        {
            var item = await _itemRepo.GetCartItemByIdAsync(cartItemId);
            if (item == null) return false;

            item.Quantity = quantity;
            await _cartRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveItemAsync(int cartItemId)
        {
            var item = await _itemRepo.GetCartItemByIdAsync(cartItemId);
            if (item == null) return false;

            await _itemRepo.RemoveCartItemAsync(item);
            await _cartRepo.SaveChangesAsync();
            return true;
        }
    }

}
