using CosmeticShop.Models;

namespace Cosmetic_Shop.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<bool> AddToFavoritesAsync(Favorite model);
        Task<bool> RemoveFromFavoritesAsync(Favorite model);
        Task<List<object>> GetFavoritesAsync(int userId);
        Task<bool?> ToggleFavoriteAsync(Favorite dto);
    }
}
