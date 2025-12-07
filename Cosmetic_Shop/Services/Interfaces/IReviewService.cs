using CosmeticShop.Models;

namespace Cosmetic_Shop.Services.Interfaces
{
    public interface IReviewService
    {
        Task<List<Review>> GetAllAsync();
        Task<Review?> GetByIdAsync(int id);
        Task<List<Review>> GetByProductIdAsync(int productId);
        Task<bool> CreateAsync(Review review);
        Task<bool> EditAsync(int id, string newContent);
        Task<bool> DeleteAsync(int id);
    }
}
