using Cosmetic_Shop.Repositories.Interfaces;
using CosmeticShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductModel = CosmeticShop.Models.Product;
namespace Cosmetic_Shop.Repositories
{

    public class CategoryRepository : ICategoryRepository
    {
        private readonly CosmeticShopContext _context;

        public CategoryRepository(CosmeticShopContext context)
        {
            _context = context;
        }

        public async Task<List<ProductModel>> GetProductsBySubcategoriesAsync(List<string> subcategories)
        {
            return await _context.Products
                .Where(p => subcategories.Contains(p.Category.ToLower()))
                .ToListAsync();
        }

    }
}
