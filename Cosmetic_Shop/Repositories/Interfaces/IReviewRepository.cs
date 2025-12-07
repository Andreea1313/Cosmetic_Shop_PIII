using CosmeticShop.Models;

namespace Cosmetic_Shop.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetAllAsync();
        Task<Review?> GetByIdAsync(int id);
        Task<List<Review>> GetByProductIdAsync(int productId);
        Task AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(Review review);
        Task<bool> ExistsAsync(int id);
        Task SaveAsync();
    }
}
