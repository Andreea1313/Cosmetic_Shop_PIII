using Cosmetic_Shop.Repositories.Interfaces;
using Cosmetic_Shop.Services.Interfaces;
using CosmeticShop.Models;

namespace Cosmetic_Shop.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepo;
        public FavoriteService(IFavoriteRepository favoriteRepo) => _favoriteRepo = favoriteRepo;

        public async Task<bool> AddToFavoritesAsync(Favorite model)
        {
            if (await _favoriteRepo.ExistsAsync(model.UserId, model.ProductId)) return false;
            await _favoriteRepo.AddFavoriteAsync(model);
            await _favoriteRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromFavoritesAsync(Favorite model)
        {
            var favorite = await _favoriteRepo.GetFavoriteAsync(model.UserId, model.ProductId);
            if (favorite == null) return false;
            await Task.Run(() => _favoriteRepo.RemoveFavoriteAsync(favorite));
            await _favoriteRepo.SaveChangesAsync();
            return true;
        }

        public async Task<List<object>> GetFavoritesAsync(int userId)
        {
            var favorites = await _favoriteRepo.GetFavoritesByUserAsync(userId); 

            return favorites
                .Where(f => f.Pressed) 
                .Select(f => new
                {
                    productId = f.ProductId,
                    name = f.Product?.Name,
                    description = f.Product?.Description,
                    image = f.Product?.ImageUrl,
                    price = f.Product?.Price,
                    pressed = true
                }).Cast<object>().ToList();
        }



        public async Task<bool?> ToggleFavoriteAsync(Favorite dto)
        {
            var favorite = await _favoriteRepo.GetFavoriteAsync(dto.UserId, dto.ProductId);

            if (favorite == null)
            {
                await _favoriteRepo.AddFavoriteAsync(new Favorite
                {
                    UserId = dto.UserId,
                    ProductId = dto.ProductId,
                    Pressed = true
                });
                await _favoriteRepo.SaveChangesAsync();
                return true;
            }
            else
            {
                favorite.Pressed = !favorite.Pressed;
                await _favoriteRepo.SaveChangesAsync();
                return favorite.Pressed;
            }
        }
    }

}
