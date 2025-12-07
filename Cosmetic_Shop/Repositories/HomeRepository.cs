using Cosmetic_Shop.Repositories.Interfaces;
using CosmeticShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cosmetic_Shop.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly CosmeticShopContext _context;

        public HomeRepository(CosmeticShopContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetTopProductsAsync(int count)
        {
            return await _context.Products.Take(count).ToListAsync();
        }

        public async Task<List<int>> GetFavoriteProductIdsAsync(int userId)
        {
            return await _context.Favorites
                .Where(f => f.UserId == userId && f.Pressed)
                .Select(f => f.ProductId)
                .ToListAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
