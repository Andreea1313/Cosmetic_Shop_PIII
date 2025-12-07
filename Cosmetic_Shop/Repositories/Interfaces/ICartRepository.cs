using CosmeticShop.Models;

namespace Cosmetic_Shop.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task AddCartAsync(Cart cart);
        Task SaveChangesAsync();
    }

    public interface ICartItemRepository
    {
        Task<CartItem> GetCartItemAsync(int cartId, int productId);
        Task<CartItem> GetCartItemByIdAsync(int cartItemId);
        Task AddCartItemAsync(CartItem item);
        Task RemoveCartItemAsync(CartItem item);
        Task RemoveAllItemsFromCartAsync(int cartId);
    }

}
