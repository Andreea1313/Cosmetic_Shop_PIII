namespace Cosmetic_Shop.Services.Interfaces
{
    public interface ICartService
    {
        Task<bool> AddToCartAsync(int userId, int productId, int quantity);
        Task<IEnumerable<object>> GetCartItemsAsync(int userId);
        Task<bool> UpdateQuantityAsync(int cartItemId, int quantity);
        Task<bool> RemoveItemAsync(int cartItemId);
        Task<bool> ClearCartAsync(int userId);
    }

}
