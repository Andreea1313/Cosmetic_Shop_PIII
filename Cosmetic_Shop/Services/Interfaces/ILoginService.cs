using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CosmeticShop.Services.Auth
{
    public interface ILoginService
    {
        Task<SignInResult> PasswordSignInAsync(string email, string password);
    }
}
