using Cosmetic_Shop.Repositories.Interfaces;
using Cosmetic_Shop.Services.Interfaces;
using CosmeticShop.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cosmetic_Shop.Services
{
    public class HomeService : IHomeService
    {
        private readonly IHomeRepository _homeRepo;
        private readonly UserManager<User> _userManager;

        public HomeService(IHomeRepository homeRepo, UserManager<User> userManager)
        {
            _homeRepo = homeRepo;
            _userManager = userManager;
        }

        public async Task<(List<Product> products, List<int> favoriteIds, int userId)> GetHomeDataAsync(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            int userId = user?.UserId ?? 0;

            var products = await _homeRepo.GetTopProductsAsync(6);
            var favoriteIds = await _homeRepo.GetFavoriteProductIdsAsync(userId);

            return (products, favoriteIds, userId);
        }

        public async Task<List<UserViewModel>> GetAllUserViewModelsAsync()
        {
            var users = await _homeRepo.GetAllUsersAsync();

            return users.Select(u => new UserViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            }).ToList();
        }
    }
}
