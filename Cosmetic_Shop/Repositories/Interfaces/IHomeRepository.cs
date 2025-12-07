using CosmeticShop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cosmetic_Shop.Repositories.Interfaces
{
    public interface IHomeRepository
    {
        Task<List<Product>> GetTopProductsAsync(int count);
        Task<List<int>> GetFavoriteProductIdsAsync(int userId);
        Task<List<User>> GetAllUsersAsync();
    }
}
