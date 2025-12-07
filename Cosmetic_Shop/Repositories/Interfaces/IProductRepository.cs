using CosmeticShop.Models;
using System.Threading.Tasks;

namespace Cosmetic_Shop.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetProductByIdAsync(int productId);
    }
}
