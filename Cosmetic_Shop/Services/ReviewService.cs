using Cosmetic_Shop.Repositories.Interfaces;
using Cosmetic_Shop.Services.Interfaces;
using CosmeticShop.Models;

namespace Cosmetic_Shop.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repo;

        public ReviewService(IReviewRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Review>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Review?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<List<Review>> GetByProductIdAsync(int productId) =>
            await _repo.GetByProductIdAsync(productId);

        public async Task<bool> CreateAsync(Review review)
        {
            if (string.IsNullOrWhiteSpace(review.Content) || review.ProductId <= 0 || review.UserId <= 0)
                return false;

            await _repo.AddAsync(review);
            return true;
        }

        public async Task<bool> EditAsync(int id, string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                return false;

            var review = await _repo.GetByIdAsync(id);
            if (review == null)
                return false;

            review.Content = newContent;
            await _repo.UpdateAsync(review);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _repo.GetByIdAsync(id);
            if (review == null)
                return false;

            await _repo.DeleteAsync(review);
            return true;
        }
    }
}
