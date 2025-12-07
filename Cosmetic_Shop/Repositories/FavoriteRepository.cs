using Cosmetic_Shop.Repositories.Interfaces;
using CosmeticShop.Models;
using Microsoft.EntityFrameworkCore;


namespace Cosmetic_Shop.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly CosmeticShopContext _context;
        public FavoriteRepository(CosmeticShopContext context) => _context = context;

        public async Task<bool> ExistsAsync(int userId, int productId)
            => await _context.Favorites.AnyAsync(f => f.UserId == userId && f.ProductId == productId);

        public async Task<Favorite> GetFavoriteAsync(int userId, int productId)
            => await _context.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

        public async Task<List<Favorite>> GetFavoritesByUserAsync(int userId)
        {
            return await _context.Favorites
                .Where(f => f.UserId == userId)
                .Include(f => f.Product)
                .ToListAsync();
        }
        public async Task AddFavoriteAsync(Favorite favorite)
            => await _context.Favorites.AddAsync(favorite);

        public async Task RemoveFavoriteAsync(Favorite favorite)
            => _context.Favorites.Remove(favorite);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
