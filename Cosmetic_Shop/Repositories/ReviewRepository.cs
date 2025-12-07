using Cosmetic_Shop.Repositories.Interfaces;
using CosmeticShop.Models;
using Microsoft.EntityFrameworkCore;

namespace Cosmetic_Shop.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly CosmeticShopContext _context;

        public ReviewRepository(CosmeticShopContext context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetAllAsync() =>
            await _context.Reviews.ToListAsync();

        public async Task<Review?> GetByIdAsync(int id) =>
            await _context.Reviews.FindAsync(id);

        public async Task<List<Review>> GetByProductIdAsync(int productId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }


        public async Task AddAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Review review)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _context.Reviews.AnyAsync(r => r.ReviewId == id);

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
