using CosmeticShop.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Cosmetic_Shop.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetCurrentUserAsync(ClaimsPrincipal principal);
        Task UpdateUserAsync(User user);
    }
}
