using Cosmetic_Shop.Repositories.Interfaces;
using CosmeticShop.Models;
using Microsoft.EntityFrameworkCore;

namespace Cosmetic_Shop.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CosmeticShopContext _context;
        public CartRepository(CosmeticShopContext context) => _context = context;

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

    public class CartItemRepository : ICartItemRepository
    {
        private readonly CosmeticShopContext _context;
        public CartItemRepository(CosmeticShopContext context) => _context = context;

        public async Task<CartItem> GetCartItemAsync(int cartId, int productId)
        {
            return await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }

        public async Task<CartItem> GetCartItemByIdAsync(int cartItemId)
        {
            return await _context.CartItems.FindAsync(cartItemId);
        }
        public async Task RemoveAllItemsFromCartAsync(int cartId)
        {
            var items = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

            _context.CartItems.RemoveRange(items);
        }
        public async Task AddCartItemAsync(CartItem item)
        {
            await _context.CartItems.AddAsync(item);
        }

        public async Task RemoveCartItemAsync(CartItem item)
        {
            _context.CartItems.Remove(item);
        }
    }

}
