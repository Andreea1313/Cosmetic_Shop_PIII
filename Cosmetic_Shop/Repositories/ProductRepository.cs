using Cosmetic_Shop.Repositories.Interfaces;
using CosmeticShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ProductModel = CosmeticShop.Models.Product;

namespace Cosmetic_Shop.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CosmeticShopContext _context;

        public ProductRepository(CosmeticShopContext context)
        {
            _context = context;
        }

        public async Task<ProductModel?> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        }
    }
}
