using CosmeticShop.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cosmetic_Shop.Services.Interfaces
{
    public interface IHomeService
    {
        Task<(List<Product> products, List<int> favoriteIds, int userId)> GetHomeDataAsync(ClaimsPrincipal principal);
        Task<List<UserViewModel>> GetAllUserViewModelsAsync();
    }

    public class UserViewModel
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
