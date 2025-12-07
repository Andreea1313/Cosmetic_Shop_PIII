using System.Threading.Tasks;
using CosmeticShop.Models;
using Cosmetic_Shop.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Identity;

namespace CosmeticShop.Services.Auth
{
    public interface IRegisterService
    {
        Task<(IdentityResult Result, User User)> RegisterUserAsync(InputModel input);
    }
}
