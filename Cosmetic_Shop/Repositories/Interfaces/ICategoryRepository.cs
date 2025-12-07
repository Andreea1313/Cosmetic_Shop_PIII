using System.Collections.Generic;
using System.Threading.Tasks;
using CosmeticShop.Models;
using ProductModel = CosmeticShop.Models.Product;
namespace Cosmetic_Shop.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<ProductModel>> GetProductsBySubcategoriesAsync(List<string> subcategories);
    }

}
