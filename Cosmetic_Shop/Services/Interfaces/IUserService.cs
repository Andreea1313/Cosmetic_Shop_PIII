using CosmeticShop.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cosmetic_Shop.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetCurrentUserAsync(ClaimsPrincipal principal);
        Task<bool> UpdateProfilePictureAsync(ClaimsPrincipal principal, IFormFile profileImage);
    }
}
