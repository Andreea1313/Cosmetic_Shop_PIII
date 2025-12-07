using CosmeticShop.Models;

namespace Cosmetic_Shop.Repositories.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<bool> ExistsAsync(int userId, int productId);
        Task<Favorite> GetFavoriteAsync(int userId, int productId);
        Task<List<Favorite>> GetFavoritesByUserAsync(int userId);
        Task AddFavoriteAsync(Favorite favorite);
        Task RemoveFavoriteAsync(Favorite favorite);
        Task SaveChangesAsync();
    }
}
